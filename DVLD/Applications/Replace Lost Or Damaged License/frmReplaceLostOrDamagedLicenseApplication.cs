﻿using Driving_License_Management.Global_Classes;
using Driving_License_Management.Licenses;
using Driving_License_Management.Licenses.Local_Driving_License;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static clsLicense;

namespace Driving_License_Management.Applications.Renew_Local_License
{
    public partial class frmReplaceLostOrDamagedLicenseApplication : Form
    {
        private int _NewLicenseID = -1;

        public frmReplaceLostOrDamagedLicenseApplication()
        {
            InitializeComponent();
        }

        private byte _GetApplicationTypeID()
        {
            // This will decide which application type to use according 

            if (rbDamagedLicense.Checked)
                return (byte)clsApplication.enApplicationType.ReplaceDamagedDrivingLicense;
            else
                return (byte)clsApplication.enApplicationType.ReplaceLostDrivingLicense;
        }

        private enIssueReason _GetIssueReason()
        {
            if (rbDamagedLicense.Checked)
                return enIssueReason.DamagedReplacement;
            else
                return enIssueReason.LostReplacement;
        }

        private void frmReplaceLostOrDamagedLicenseApplication_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;

            //rbDamagedLicense.Checked = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmReplaceLostOrDamagedLicenseApplication_Activated(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.txtLicenseIDFocus();
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (frmShowPersonLicenseHistory ShowPersonLicenseHistory = new frmShowPersonLicenseHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID))
            {
                ShowPersonLicenseHistory.ShowDialog();
            }

        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (frmShowLicenseInfo ShowLicenseInfo = new frmShowLicenseInfo(_NewLicenseID))
            {
                ShowLicenseInfo.ShowDialog();
            }

        }

        private void rbDamagedLicense_CheckedChanged(object sender, EventArgs e)
        {
            lblTitle.Text = "Replacement for Damaged License";
            this.Text = lblTitle.Text;

            lblApplicationFees.Text = clsApplicationType.Find(_GetApplicationTypeID()).Fees.ToString();
        }

        private void rbLostLicense_CheckedChanged(object sender, EventArgs e)
        {
            lblTitle.Text = "Replacement for Lost License";
            this.Text = lblTitle.Text;

            lblApplicationFees.Text = clsApplicationType.Find(_GetApplicationTypeID()).Fees.ToString();
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int SelectedLicenseID = obj;

            lblOldLicenseID.Text = SelectedLicenseID.ToString();
            llShowLicenseHistory.Enabled = (SelectedLicenseID != -1);

            if (SelectedLicenseID == -1)
            {
                return;
            }

            //dont allow a replacement if is Active .
            if (!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("Selected License is not Not Active, choose an active license."
                    , "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIssueReplacement.Enabled = false;
                return;
            }

            btnIssueReplacement.Enabled = true;
        }

        private void btnIssueReplacement_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Are you sure you want to Issue a Replacement for the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }


            clsLicense NewLicense = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.Replace(_GetIssueReason(), clsGlobal.CurrentUser.UserID);

            if (NewLicense == null)
            {
                MessageBox.Show("Faild to Issue a replacemnet for this  License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            lblApplicationID.Text = NewLicense.ApplicationID.ToString();
            _NewLicenseID = NewLicense.LicenseID;

            lblRreplacedLicenseID.Text = _NewLicenseID.ToString();

            MessageBox.Show("Licensed Replaced Successfully with ID=" + _NewLicenseID.ToString(), "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnIssueReplacement.Enabled = false;
            gbReplacementFor.Enabled = false;

            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            llShowLicenseInfo.Enabled = true;

        }
      
        
    }


}
