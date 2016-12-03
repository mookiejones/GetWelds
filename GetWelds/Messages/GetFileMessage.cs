using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Ookii.Dialogs.Wpf;
using GetWelds.Robots;
using System.Collections.ObjectModel;

namespace GetWelds.Messages
{
    public class GetFileMessage:MessageBase
    {

        public Action<FileMessage> Callback { get; set; }

        public FileMessage Message { get; set; }


        public GetFileMessage(FileMessage msg, Action<FileMessage> callback):base(String.Empty,callback)
        {
            Message = msg;
            this.Callback = callback;
        }

     



        public void GetMessage(Window owner)
        {
            switch (Message.MessageType)
            {
                case MessageType.Open:
                    Message.IsValid = Message.OpenFileDialog.ShowDialog(owner) == true;
                    if (Message.IsValid)
                    Message.FileName = Message.OpenFileDialog.FileName;
                    break;
                case MessageType.Save:
                    Message.IsValid = Message.SaveFileDialog.ShowDialog(owner) == true;
                    if (Message.IsValid)
                    {
                        Message.FileName = Message.SaveFileDialog.FileName + ".xml";

                    }
                    break;
                default:
                    return;
            }


            Callback(Message);

        }

    }

    public class FileMessage
    {

        public FileMessage() { }
        public FileMessage(string title, string filter,MessageType type, bool multiselect=false)
        {

            MessageType = type;

            switch (MessageType)
            {
                case Messages.MessageType.Open:
                    OpenFileDialog = new VistaOpenFileDialog
                    {
                        Filter = filter,
                        Title = title,
                        Multiselect = multiselect
                    };

                    break;
                case Messages.MessageType.Save:
          
            SaveFileDialog = new VistaSaveFileDialog
            {
                Filter = filter,
                Title = title
               
            };
            break;
            }
        }



        public ObservableCollection<AbstractRobot> Robots { get; set; }
        public VistaSaveFileDialog SaveFileDialog { get; set; }
        public  VistaOpenFileDialog OpenFileDialog { get; set; }

           public string Result { get; set; }
           public bool IsValid { get; set; }

           public string FileName { get; set; }

           public MessageType MessageType { get; set; }
     
    }

    public enum MessageType { Open,Save}
}
