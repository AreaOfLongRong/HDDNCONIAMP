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


        #region ***********全局变量*************  
        int start = 0;
        int sun = 0;
        int count = 0;
        #endregion


        /// <summary>
        /// 与之绑定关联的富文本内容
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
                    //如果还想再找一遍,添加下面这句  
                    //rbox.SelectionStart = rbox.Text.Length;  
                }
            }
        }


        /// <summary> 消息提示,提示用户查找结果<para>　<para>  
        /// 参数1(str):用户指定要查找的字符串</para></para> </summary>  
        /// <param name="str">用户指定要查找的字符串</param>  
        private void seeks(string str)
        {
            if (sun != 0)
                MessageBox.Show(string.Format("已查找完毕,共〖{0}〗个 \"{1}\"！", sun, str), "查找 - 温馨提示");
            else MessageBox.Show(string.Format("没有查找到 \"{0}\"！", str), "查找 - 温馨提示");
        }
    }
}