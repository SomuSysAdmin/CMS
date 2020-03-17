using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BT_Escalation_Detector
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void detect_Click(object sender, EventArgs e)
        {
            int esc_lvl = -1;
            String[] esc_matrix = new String[] { "@bt.com", "sl-gsd@bt.com", "dutymanager.gur@bt.com",  "isha.2.singh@bt.com; subbin.sharda@bt.com", "global.escalation.management@bt.com" };
            String[] esc_owner = new String[] { null, "Shift Lead @BT", "Duty Manager @BT", "Operations Manager @BT", "Global Escalaton Managemet @BT" };

            recipients_box.Text = Clipboard.GetText();
            if (recipients_box.Text == "") {
                MessageBox.Show("Please copy the 'to', 'cc' and 'bcc' fields from the mail!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                String esc_text = (String) recipients_box.Text;
                for (int i = esc_matrix.Length-1; i>=0; i--)
                {
                    if (esc_text.ToLower().Contains(esc_matrix[i].ToLower()))
                    {
                        // if one of the BT Escalation email addrs is found
                        esc_lvl = i;
                        break;
                    }
                }

                if (esc_lvl == -1)
                {
                    MessageBox.Show("Please copy the 'to', 'cc' and 'bcc' fields from the mail!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    clear_recipients.PerformClick();
                    return;
                }

                esc_meter_label.Text = esc_lvl + "/4";
                if (esc_lvl == 0)
                {
                    nxt_esc_label.Text = esc_matrix[esc_lvl + 1];
                }
                else if (esc_lvl > 0 && esc_lvl < 4)
                {
                    cur_esc_label.Text = esc_matrix[esc_lvl];
                    nxt_esc_label.Text = esc_matrix[esc_lvl + 1];
                    esc_banner.Text = "*** Current escalation level = " + esc_owner[esc_lvl] +" ***";
                }
                else if (esc_lvl == 4)
                {
                    cur_esc_label.Text = nxt_esc_label.Text = esc_matrix[esc_lvl];
                    esc_banner.Text = "*** Current escalation level = " + esc_owner[esc_lvl] + " ***";
                }
            }
        }

        private void clear_recipients_Click(object sender, EventArgs e)
        {
            recipients_box.Text = esc_banner.Text = "";
            cur_esc_label.Text = "None";
            nxt_esc_label.Text = "None";
            esc_meter_label.Text = "?/4";
        }

        private void cur_esc_label_DoubleClick(object sender, EventArgs e)
        {
            if (cur_esc_label.Text != "None")
            {
                Clipboard.SetText(cur_esc_label.Text);
                CopyMsg();
            }
        }

        private void nxt_esc_label_DoubleClick(object sender, EventArgs e)
        {
            if (nxt_esc_label.Text != "None")
            {
                Clipboard.SetText(nxt_esc_label.Text);
                CopyMsg();
            }
        }

        private void CopyMsg()
        {
            Task.Factory.StartNew(() =>
            {
                String copy_msg = "Copied to Clipboard!";
                if (toolStripStatusLabel1.Text != copy_msg)
                {
                    String org_text = toolStripStatusLabel1.Text;
                    toolStripStatusLabel1.Text = copy_msg;
                    System.Threading.Thread.Sleep(1500);
                    toolStripStatusLabel1.Text = org_text;
                }
            });
        }

        private void esc_banner_DoubleClick(object sender, EventArgs e)
        {
            Clipboard.SetText(esc_banner.Text);
            CopyMsg();
        }
    }
}
