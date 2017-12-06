using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Botwave.DynamicForm.Extension.Domain
{
    /// <summary>
    /// JS库实体类
    /// </summary>
    public class JsLibrary
    {
        private int id;
        private string title;
        private string fileName;
        private int type;
        private string events;
        private string function;
        private string creator;
        private DateTime createdTime;

        public int Id {
            get { return id; }
            set { id = value; }
        }

        public string Title {
            get { return title; }
            set { title = value; }
        }

        public string FileName {
            get { return fileName; }
            set { fileName = value; }
        }

        public int Type {
            get { return type; }
            set { type = value; }
        }

        public string Function { 
            get { return function; }
            set { function = value; }
        }

        public string Events {
            get { return events; }
            set { events = value; }
        }

        public string Creator {
            get { return creator; }
            set { creator = value; }
        }

        public DateTime CreatedTime {
            get { return createdTime; }
            set { createdTime = value; }
        }
    }
}
