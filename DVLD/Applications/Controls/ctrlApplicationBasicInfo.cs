﻿using Driving_License_Management.PeopleFs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Driving_License_Management.Applications.Controls
{
    public partial class ctrlApplicationBasicInfo : UserControl
    {

        private clsApplication _Application;

        private int _ApplicationID = -1;

        public ctrlApplicationBasicInfo()
        {
            InitializeComponent();
        }    

        public int ApplicationID
        {
            get { return _ApplicationID; }
        }

        public void LoadApplicationInfo(int ApplicationID)
        {
            _Application = clsApplication.FindBaseApplication(ApplicationID);

            if (_Application == null)
            {
                ResetApplicationInfo();
                MessageBox.Show("No Application with ApplicationID = " + ApplicationID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                _FillApplicationInfo();
        }

        public void LoadApplicationInfo(ref clsApplication ApplicationInfo)
        {
            _Application = ApplicationInfo;

            if (_Application == null)
            {
                ResetApplicationInfo();
                MessageBox.Show("No Application with ApplicationID = " + ApplicationID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                _FillApplicationInfo();
            }
        }

        private void _FillApplicationInfo()
        {
            _ApplicationID = _Application.ApplicationID;

            lblApplicationID.Text = _Application.ApplicationID.ToString();
            lblStatus.Text = _Application.StatusText;
            lblType.Text = _Application.ApplicationTypeInfo.Title;
            lblFees.Text = _Application.PaidFees.ToString();
            lblApplicant.Text = _Application.ApplicantFullName;
            lblDate.Text = clsFormat.DateToShort(_Application.ApplicationDate);
            lblStatusDate.Text = clsFormat.DateToShort(_Application.LastStatusDate);
            lblCreatedByUser.Text = _Application.CreatedByUserInfo.UserName;
        }

        public void ResetApplicationInfo()
        {
            _ApplicationID = -1;

            lblApplicationID.Text = "[????]";
            lblStatus.Text = "[????]";
            lblType.Text = "[????]";
            lblFees.Text = "[????]";
            lblApplicant.Text = "[????]";
            lblDate.Text = "[????]";
            lblStatusDate.Text = "[????]";
            lblCreatedByUser.Text = "[????]";
        }
     
        private void llViewPersonInfo_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (frmShowDetails frm = new frmShowDetails(_Application.ApplicantPersonID))
            {
                frm.DataBack += _FillApplicationInfo;

                frm.ShowDialog();
            }
            
        }


    }

}
