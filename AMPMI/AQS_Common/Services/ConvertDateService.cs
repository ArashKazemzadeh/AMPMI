﻿using System;
using System.Globalization;

public static class ConvertDateService
{
    /// <summary>
    /// تبدیل تاریخ میلادی به شمسی (DateTime به DateTime)
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    public static DateTime ToPersianDate(this DateTime date)
    {
        PersianCalendar persianCalendar = new PersianCalendar();
        int year = persianCalendar.GetYear(date);
        int month = persianCalendar.GetMonth(date);
        int day = persianCalendar.GetDayOfMonth(date);

        return new DateTime(year, month, day);
    }

    /// <summary>
    /// تبدیل تاریخ شمسی به میلادی (DateTime به DateTime)
    /// </summary>
    /// <param name="persianDate"></param>
    /// <returns></returns>
    public static DateTime ToGregorianDate(this DateTime persianDate)
    {
        PersianCalendar persianCalendar = new PersianCalendar();
        int year = persianCalendar.GetYear(persianDate);
        int month = persianCalendar.GetMonth(persianDate);
        int day = persianCalendar.GetDayOfMonth(persianDate);

        return new DateTime(year, month, day, persianCalendar);
    }
}