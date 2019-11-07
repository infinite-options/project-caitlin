using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using ProjectCaitlin.Helper;

namespace ProjectCaitlin
{
    [Activity(Label = "LongTerm", Theme ="@style/Theme.AppCompat.Light")]
    public class LongTerm : AppCompatActivity
    {

        EditText taskEditText;
        DbHelper db;
        CustomAdapter adapter;
        ListView lstTask;

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_item, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_add:
                    taskEditText = new EditText(this);
                    Android.Support.V7.App.AlertDialog dialog = new Android.Support.V7.App.AlertDialog.Builder(this)
                        .SetTitle("Add New Task")
                        .SetMessage("What do you want to do next?")
                        .SetView(taskEditText)
                        .SetPositiveButton("Add", OkAction)
                        .SetNegativeButton("Cancel", CancelAction)
                        .Create();
                    dialog.Show();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void CancelAction(object sender, DialogClickEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OkAction(object sender, DialogClickEventArgs e)
        {
            string task = taskEditText.Text;
            db.InsertNewTask(task);

            LoadTaskList();
        }

        public void LoadTaskList()
        {
            List<string> taskList = db.getTaskList();
            adapter = new CustomAdapter(this, taskList, db);
            lstTask.Adapter = adapter;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LongTerm);
            db = new Helper.DbHelper(this);
            lstTask = FindViewById<ListView>(Resource.Id.lstTask);

            LoadTaskList();
        }
    }
}