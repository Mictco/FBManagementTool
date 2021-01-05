using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
namespace FirebaseManagement
{
    public partial class FB_Management : Form
    {
        #region Private Variables
        string AuthSecret = "gQln6cPDx07ObhbkFPQ4ZkfGP1Bvqnp2NUb5UIuq";
        string BasePath = "https://mictcoexeupdate-default-rtdb.firebaseio.com/";
        string sTable = "UpdatesLink/";
        IFirebaseClient client;
        IFirebaseConfig config;
        UpdateItem item;
        #endregion

        #region Constructor
        public FB_Management()
        {
            InitializeComponent();
        }
        #endregion

        #region Methods
        private void firebaseConfiqration()
        {
            config = new FirebaseConfig
            {
                AuthSecret = AuthSecret,
                BasePath = BasePath
            };
            client = new FireSharp.FirebaseClient(config);
        }
        #endregion

        #region Events
        private void FB_Management_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        private void dgvDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex>=0)
            {
                item = (UpdateItem)dgvDetails.CurrentRow.DataBoundItem;
                txtLink.Text = item.UpdationLink;
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
                txtLink.Focus();
                txtLink.SelectAll();
            }
        }
        private void FB_Management_Load(object sender, EventArgs e)
        {
            List<UpdateItem> updateItems = new List<UpdateItem>();
            firebaseConfiqration();
            FirebaseResponse response = client.Get(sTable);
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            if (data != null)
            {
                foreach (var item in data)
                {
                    updateItems.Add(JsonConvert.DeserializeObject<UpdateItem>(((JProperty)item).Value.ToString()));
                }
            }
            if (updateItems.Count() > 0)
            {
                dgvDetails.DataSource = updateItems;
                
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtLink.Text == string.Empty) { MessageBox.Show("No link selected to Update."); return; }
            firebaseConfiqration();
            if (btnSave.Text=="Save")
            {
                UpdateItem mictco = new UpdateItem();
                mictco.UpdationLink = txtLink.Text;
                var entdata = mictco;
                PushResponse responses = client.Push(sTable, entdata);
                entdata.Id = responses.Result.name;
                SetResponse setResponse = client.Set(sTable + entdata.Id, entdata);
                MessageBox.Show("Link Saved");
            }
            else
            {
                item.UpdationLink = txtLink.Text;
                FirebaseResponse responses = client.Update(sTable + item.Id, item);
                MessageBox.Show("Link Updated");
            }
            FB_Management_Load(null, null);
            txtLink.Text = string.Empty;
            this.Close();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtLink.Text == string.Empty) { MessageBox.Show("No link selected to delete."); return; }
            firebaseConfiqration();
            FirebaseResponse response = client.Delete(sTable + item.Id);
            dgvDetails.DataSource = null;
            txtLink.Text = string.Empty;
            btnDelete.Enabled = false;
            btnSave.Text = "Save";
            txtLink.Focus();
        }
        #endregion
    }
}
