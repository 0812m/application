using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PH.ToolsLibrary.Extension;
using PH.Wechat.OfficialAccounts.Message.From;
using PH.Wechat.OfficialAccounts.Message.To;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PH.Wechat.OfficialAccounts.Message.Handler
{
    public class WxMessageHandle : IWxMessageHandle
    {
        private readonly IMessagePool _messagePool;
        private readonly IServiceProvider _provider;
        private readonly ILogger<WxMessageHandle> _logger;

        public WxMessageHandle
            (
            IMessagePool messagePool,
            IServiceProvider provider,
            ILogger<WxMessageHandle> logger
            )
        {
            _messagePool = messagePool;
            _provider = provider;
            _logger = logger;
        }

        public async Task<ToMessageBase> HandlerAsync(HttpContext httpcontext)
        {
            #region 解决报错：Synchronous operations are disallowed. Call ReadAsync or set AllowSynchronousIO to true instead.
            //var syncIOFeature = httpcontext.Features.Get<IHttpBodyControlFeature>();
            //if (syncIOFeature != null)
            //{
            //    syncIOFeature.AllowSynchronousIO = true;
            //}
            #endregion

            using var reader = new StreamReader(httpcontext.Request.Body, Encoding.UTF8);
            var xml = await reader.ReadToEndAsync();
            _logger.LogInformation($"接收到微信消息推送：{xml}");
            var xmlDoc = XElement.Parse(xml);
            if (xmlDoc is null)
                throw new ArgumentNullException(nameof(xmlDoc));

            var msgtype = xmlDoc.Element("MsgType")?.Value?.ToUpper();
            var fromUserName = xmlDoc.Element("FromUserName")?.Value;
            int.TryParse(xmlDoc.Element("MsgId")?.Value, out var msgId);
            int.TryParse(xmlDoc.Element("CreateTime")?.Value, out var createTime);

            //消息排重
            if (!_messagePool.TryAdd(new MessageFormatBase()
            {
                FromUserName = fromUserName,
                MessageId = msgId,
                CreateTime = createTime
            }, msgId == 0))
                return new ToNullMessage();

            if (msgtype == "EVENT")
            {
                var eventMsg = xmlDoc.Element("Event")?.Value;
                    EventType eventType = (EventType)Enum.Parse(typeof(EventType), eventMsg, true);
                    return eventType switch
                    {
                        EventType.UnSubscribe => await ExecutionAsync<ISubscribeEventHanlder, SubscribeEvent>(xml),
                        EventType.Subscribe => await ExecutionAsync<ISubscribeEventHanlder, SubscribeEvent>(xml),
                        EventType.Scan => await ExecutionAsync<IScanQRCodeEventHandler, ScanQRCodeEvent>(xml),
                        EventType.Location => await ExecutionAsync<ILocationEventHanlder, LocationEvent>(xml),
                        EventType.Click => await ExecutionAsync<IClickEventHandler, ClickEvent>(xml),
                        _ => throw new ArgumentOutOfRangeException(),
                    };
            }
            else
            {
                MessageType type = (MessageType)Enum.Parse(typeof(MessageType), msgtype, true);
                return type switch
                {
                    MessageType.Text => await ExecutionAsync<ITextMessageHandler, TextMessage>(xml),
                    MessageType.Image => await ExecutionAsync<IImageMessageHanlder, ImageMessage>(xml),
                    MessageType.Voice => await ExecutionAsync<IVoiceMessageHandler, VoiceMessage>(xml),
                    MessageType.Video => await ExecutionAsync<IVideoMessageHandler, VideoMessage>(xml),
                    MessageType.ShortVideo => await ExecutionAsync<IShortVideoMessageHandler, ShortVideoMessage>(xml),
                    MessageType.Location => await ExecutionAsync<ILocationMessageHandler, LocationMessage>(xml),
                    MessageType.Link => await ExecutionAsync<ILinkMessageHandler, LinkMessage>(xml),
                    _ => throw new ArgumentOutOfRangeException(),
                };
            }
        }

    public async Task<ToMessageBase> ExecutionAsync<TService, TParam>(string xml)
        where TService : IMessageHnadlerBase
        where TParam : MessageFormatBase, new()
    {
        var handler = _provider.GetService<TService>();
        if (handler is null)
            throw new InvalidOperationException($"处理微信消息应用程序：{typeof(TService)}未注册");

        var model = xml.XmlToModel<TParam>();
        return await handler.ExecuteAsync(model);
    }
}
}
