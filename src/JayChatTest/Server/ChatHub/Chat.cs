using System;
using System.Collections.Generic;
using System.Dynamic;
using WebSocketSharp;
using WebSocketSharp.Server;
using Newtonsoft.Json;

namespace websocket_sharp_test.ChatHub
{
    class Chat : WebSocketBehavior
    {
        enum RequestTypes
        {
            RegisterUser,
            SendMessage,

        }

        static Dictionary<string, string> nameList = new Dictionary<string, string>();

        protected override void OnOpen()
        {
            base.OnOpen();
        }

        protected override void OnMessage(MessageEventArgs e)
        {

            dynamic message = JsonConvert.DeserializeObject(e.Data);

            if (message.requestTypeId == RequestTypes.RegisterUser)
            {
                nameList.Add(this.ID, message.username.ToString());
                SendMessage("System", $"{message.username} just connected");
            }
            else
                SendMessage(nameList[this.ID], message.message.ToString());

        }

        protected override void OnClose(CloseEventArgs e)
        {
            base.OnClose(e);
            SendMessage("System", $"{nameList[this.ID]} has left the chat!");
            nameList.Remove(this.ID);

        }

        private void SendMessage(string username, string message)
        {
            dynamic mess = new ExpandoObject();
            mess.message = message;
            mess.username = username;

            Sessions.Broadcast(JsonConvert.SerializeObject(mess));
        }
    }
}
