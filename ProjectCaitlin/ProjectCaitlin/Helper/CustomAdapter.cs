using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace ProjectCaitlin.Helper
{
    public class CustomAdapter : BaseAdapter
    {
        private LongTerm longTerm;
        private List<string> taskList;
        private DbHelper db;

        public CustomAdapter(LongTerm longTerm, List<string> taskList, DbHelper db)
        {
            this.longTerm = longTerm;
            this.taskList = taskList;
            this.db = db;
        }

        public override int Count
        {
            get
            {
                return taskList.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            LayoutInflater inflater = (LayoutInflater)longTerm.GetSystemService(Context.LayoutInflaterService);
            View view = inflater.Inflate(Resource.Layout.row, null);

            TextView txtTask = view.FindViewById<TextView>(Resource.Id.task_title);
            Button btnDelete = view.FindViewById<Button>(Resource.Id.btnDelete);

            txtTask.Text = taskList[position];
            btnDelete.Click += delegate
            {
                string task = taskList[position];
                db.deleteTask(task);
                longTerm.LoadTaskList();
            };
            return view;
        }
    }
}