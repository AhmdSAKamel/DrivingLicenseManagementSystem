﻿using DVLDBuiness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



public class clsDriver
{
    public enum enMode { AddNew = 0, Update };
    public enMode Mode = enMode.AddNew;

    public clsPerson PersonInfo;

    public int DriverID { set; get; }
    public int PersonID { set; get; }
    public short CreatedByUserID { set; get; }
    public DateTime CreatedDate { get; }
    
    public clsDriver()
    {
        this.DriverID = -1;
        this.PersonID = -1;
        this.CreatedByUserID = -1;
        this.CreatedDate = DateTime.Now;

        Mode = enMode.AddNew;
    }

    private clsDriver(int DriverID, int PersonID, short CreatedByUserID, DateTime CreatedDate)
    {
        this.DriverID = DriverID;
        this.PersonID = PersonID;
        this.CreatedByUserID = CreatedByUserID;
        this.CreatedDate = CreatedDate;
        this.PersonInfo = clsPerson.Find(PersonID);

        Mode = enMode.Update;
    }

    private bool _AddNewDriver()
    {
        this.DriverID = DriverData.AddNewDriver(PersonID, CreatedByUserID);

        return (this.DriverID != -1);
    }

    private bool _UpdateDriver()
    {
        return DriverData.UpdateDriver(this.DriverID, this.PersonID, this.CreatedByUserID);
    }

    public static clsDriver FindByDriverID(int DriverID)
    {
        int PersonID = -1; short CreatedByUserID = -1; DateTime CreatedDate = DateTime.Now;

        if (DriverData.GetDriverInfoByDriverID(DriverID, ref PersonID, ref CreatedByUserID, ref CreatedDate))

            return new clsDriver(DriverID, PersonID, CreatedByUserID, CreatedDate);
        else
            return null;

    }

    public static clsDriver FindByPersonID(int PersonID)
    {
        int DriverID = -1; short CreatedByUserID = -1; DateTime CreatedDate = DateTime.Now;

        if (DriverData.GetDriverInfoByPersonID(PersonID, ref DriverID, ref CreatedByUserID, ref CreatedDate))

            return new clsDriver(DriverID, PersonID, CreatedByUserID, CreatedDate);
        else
            return null;
    }

    public static DataTable GetAllDrivers()
    {
        return DriverData.GetAllDrivers();
    }

    public bool Save()
    {
        switch (Mode)
        {
            case enMode.AddNew:
                if (_AddNewDriver())
                {
                    Mode = enMode.Update;
                    return true;
                }
                else
                {
                    return false;
                }

            case enMode.Update:

                return _UpdateDriver();

        }

        return false;
    }

    public static DataTable GetLicenses(int DriverID)
    {
        return clsLicense.GetDriverLicenses(DriverID);
    }

    public static DataTable GetInternationalLicenses(int DriverID)
    {
        return clsInternationalLicense.GetDriverInternationalLicenses(DriverID);
    }

}

