using System;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;

namespace HDDNCONIAMP.UI.UserSettings
{
    public partial class FSearchLog : DevComponents.DotNetBar.OfficeForm
    {
        public FSearchLog()
        {
            InitializeComponent();
        }


        #region ***********ȫ�ֱ���*************  
        int start = 0;
        int sun = 0;
        int count = 0;
        #endregion


        /// <summary>
        /// ��֮�󶨹����ĸ��ı�����
        /// </summary>
        public RichTextBoxEx BindingRTBE { get; set; }

        private void buttonXLogSearch_Click(object sender, EventArgs e)
        {
            string searchStr = textBoxXSearchText.Text;
            if (BindingRTBE == null)
                return;
            if (radioButtonSearchDown.Checked)
            {
                int rboxL = BindingRTBE.Text.Length;
                
                if (start < rboxL)
                {
                    start = BindingRTBE.Find(searchStr, start, RichTextBoxFinds.None);
                    int los = BindingRTBE.SelectionStart + searchStr.Length;


                    if ((start < 0) || (start > rboxL))
                    {
                        if (count == 0)
                        {
                            this.seeks(searchStr);
                            start = los;
                            sun = 0;
                        }
                        else
                        {
                            this.seeks(searchStr);
                            start = los;
                            sun = 0;
                        }
                    }
                    else if (start == rboxL || start < 0)
                    {
                        this.seeks(searchStr);
                        start = los;
                        sun = 0;
                    }
                    else
                    {
                        sun++;
                        start = los;
                        BindingRTBE.Focus();
                    }
                }
                else if (start == rboxL || start < 0)
                {
                    int los = BindingRTBE.SelectionStart + searchStr.Length;
                    this.seeks(searchStr);
                    start = los;
                    sun = 0;
                }
                else
                {
                    int los = BindingRTBE.SelectionStart + searchStr.Length;
                    this.seeks(searchStr);
                    start = los;
                    sun = 0;
                }
            }
            else if (radioButtonSearchUP.Checked)
            {
                int rboxL = BindingRTBE.SelectionStart;
                int index = BindingRTBE.Find(searchStr, 0, rboxL, RichTextBoxFinds.Reverse);
                if (index > 0)
                {
                    BindingRTBE.SelectionStart = index;
                    BindingRTBE.SelectionLength = searchStr.Length;
                    sun++;
                    BindingRTBE.Focus();
                }
                else if (index < 0)
                {
                    seeks(searchStr);
                    sun = 0;
                    //�����������һ��,����������  
                    //rbox.SelectionStart = rbox.Text.Length;  
                }
            }
        }


        /// <summary> ��Ϣ��ʾ,��ʾ�û����ҽ��<para>��<para>  
        /// ����1(str):�û�ָ��Ҫ���ҵ��ַ���</para></para> </summary>  
        /// <param name="str">�û�ָ��Ҫ���ҵ��ַ���</param>  
        private void seeks(string str)
        {
            if (sun != 0)
                MessageBox.Show(string.Format("�Ѳ������,����{0}���� \"{1}\"��", sun, str), "���� - ��ܰ��ʾ");
            else MessageBox.Show(string.Format("û�в��ҵ� \"{0}\"��", str), "���� - ��ܰ��ʾ");
        }
    }
}