﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SigmaChart.CmdsMenu
{
    public partial class CalendarForm : Form
    {
        public DateTime selectedDate;

        public CalendarForm()
        {
            InitializeComponent();

            selectedDate = DateTime.Now;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
        
        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            selectedDate = monthCalendar1.SelectionEnd;
        }
    }
}