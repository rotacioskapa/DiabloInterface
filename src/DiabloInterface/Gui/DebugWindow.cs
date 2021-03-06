﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DiabloInterface
{
    public partial class DebugWindow : Form
    {
        Label[] actlabels;
        Label[,] questlabels;
        Label[,,] questBits;

        Label[] actlabelsNightmare;
        Label[,] questlabelsNightmare;
        Label[,,] questBitsNightmare;

        Label[] actlabelsHell;
        Label[,] questlabelsHell;
        Label[,,] questBitsHell;

        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }
        
        public DebugWindow()
        {
            InitializeComponent();
            MinimizeBox = false;
            MaximizeBox = false;
        }
        private byte reverseBits(byte b)
        {
            return (byte)(((b * 0x80200802ul) & 0x0884422110ul) * 0x0101010101ul >> 32);
        }

        public void setQuestDataNormal(byte[] questBuffer)
        {
            setQuestData(questBuffer, 0);
        }

        public void setQuestDataNightmare(byte[] questBuffer)
        {
            setQuestData(questBuffer, 1);
        }

        public void setQuestDataHell(byte[] questBuffer)
        {
            setQuestData(questBuffer, 2);
        }
        public void setQuestData(byte[] questBuffer, int difficulty)
        {
            Label[,,] bitlabels;
            switch (difficulty)
            {
                case 2:
                    bitlabels = questBitsHell; break;
                case 1:
                    bitlabels = questBitsNightmare; break;
                case 0:
                default:
                    bitlabels = questBits; break;
            }

            short value;
            int act, quest;
            for (int i = 0; i < questBuffer.Length - 1; i += 2)
            {
                value = BitConverter.ToInt16(new byte[2] { reverseBits(questBuffer[i]), reverseBits(questBuffer[i + 1]), }, 0);
                switch ((D2Data.Quest)i)
                {
                    case D2Data.Quest.A1Q1:
                        act = 1;quest = 1;
                        break;
                    case D2Data.Quest.A1Q2:
                        act = 1; quest = 2;
                        break;
                    case D2Data.Quest.A1Q3:
                        act = 1; quest = 3;
                        break;
                    case D2Data.Quest.A1Q4:
                        act = 1; quest = 4;
                        break;
                    case D2Data.Quest.A1Q5:
                        act = 1; quest = 5;
                        break;
                    case D2Data.Quest.A1Q6:
                        act = 1; quest = 6;
                        break;

                    case D2Data.Quest.A2Q1:
                        act = 2; quest = 1;
                        break;
                    case D2Data.Quest.A2Q2:
                        act = 2; quest = 2;
                        break;
                    case D2Data.Quest.A2Q3:
                        act = 2; quest = 3;
                        break;
                    case D2Data.Quest.A2Q4:
                        act = 2; quest = 4;
                        break;
                    case D2Data.Quest.A2Q5:
                        act = 2; quest = 5;
                        break;
                    case D2Data.Quest.A2Q6:
                        act = 2; quest = 6;
                        break;

                    case D2Data.Quest.A3Q1:
                        act = 3; quest = 1;
                        break;
                    case D2Data.Quest.A3Q2:
                        act = 3; quest = 2;
                        break;
                    case D2Data.Quest.A3Q3:
                        act = 3; quest = 3;
                        break;
                    case D2Data.Quest.A3Q4:
                        act = 3; quest = 4;
                        break;
                    case D2Data.Quest.A3Q5:
                        act = 3; quest = 5;
                        break;
                    case D2Data.Quest.A3Q6:
                        act = 3; quest = 6;
                        break;

                    case D2Data.Quest.A4Q1:
                        act = 4; quest = 1;
                        break;
                    case D2Data.Quest.A4Q2:
                        act = 4; quest = 2;
                        break;
                    case D2Data.Quest.A4Q3:
                        act = 4; quest = 3;
                        break;

                    case D2Data.Quest.A5Q1:
                        act = 5; quest = 1;
                        break;
                    case D2Data.Quest.A5Q2:
                        act = 5; quest = 2;
                        break;
                    case D2Data.Quest.A5Q3:
                        act = 5; quest = 3;
                        break;
                    case D2Data.Quest.A5Q4:
                        act = 5; quest = 4;
                        break;
                    case D2Data.Quest.A5Q5:
                        act = 5; quest = 5;
                        break;
                    case D2Data.Quest.A5Q6:
                        act = 5; quest = 6;
                        break;

                    default:
                        act = 0;
                        quest = 0;
                        break;
                }
                if (act > 0 && quest > 0)
                {
                    for ( int x = 0; x < 16; x++ )
                    {

                        bitlabels[act - 1, quest - 1, x].Invoke(new Action(delegate () {
                            if (isNthBitSet(value, x))
                            {
                                if (bitlabels[act - 1, quest - 1, x].BackColor != Color.GreenYellow)
                                {
                                    bitlabels[act - 1, quest - 1, x].BackColor = Color.GreenYellow;
                                }
                            } else if (bitlabels[act - 1, quest - 1, x].BackColor != Color.LightGray)
                            {
                                bitlabels[act - 1, quest - 1, x].BackColor = Color.LightGray;
                            }
                        }));
                    }
                }
            }
        }
        public void updateAutosplits (List<AutoSplit> autosplits)
        {

            int y = 0;
            panel1.Controls.Clear();
            foreach (AutoSplit autosplit in autosplits)
            {
                Label lbl = new Label();
                lbl.SetBounds(0, y, panel1.Bounds.Width, 16);
                panel1.Controls.Add(lbl);
                autosplit.bindControl(lbl);
                y += 16;
            }
        }
        private bool isNthBitSet(short c, int n)
        {
            int[] mask = { 128, 64, 32, 16, 8, 4, 2, 1 };
            if (n >= 8)
            {
                c = (short)((int)c >> 8);
                n -= 8;
            }
            return ((c & mask[n]) != 0);
        }

        private void DebugWindow_Load(object sender, EventArgs e)
        {
            actlabels = new Label[5];
            questlabels = new Label[5,6];
            questBits = new Label[5, 6, 16];
            for ( int i = 0; i < 5; i ++ )
            {
                actlabels[i] = new Label();
                actlabels[i].Text = "Act " + (i + 1);
                actlabels[i].SetBounds(20, i * 100, 40, 16);
                this.tabPage1.Controls.Add(actlabels[i]);
                for ( int j = 0; j< (i == 3 ? 3:6); j++ )
                {
                    questlabels[i, j] = new Label();
                    questlabels[i, j].Text = "Quest " + (j + 1);
                    questlabels[i, j].SetBounds(100, i * 100+(j * 16), 80, 16);
                    this.tabPage1.Controls.Add(questlabels[i, j]);
                    for (int k = 0; k < 16; k++)
                    {
                        questBits[i, j, k] = new Label();
                        questBits[i, j, k].Text = ""+k;
                        questBits[i, j, k].Font = new Font(Font.SystemFontName, 6.5f);
                        questBits[i, j, k].SetBounds(100 +80+ (k)*24, i * 100 + (j * 16), 20, 14);
                        questBits[i, j, k].BorderStyle = BorderStyle.FixedSingle;
                        questBits[i, j, k].BackColor = Color.LightGray;
                        this.tabPage1.Controls.Add(questBits[i, j, k]);
                    }
                }
            }
            actlabelsNightmare = new Label[5];
            questlabelsNightmare = new Label[5, 6];
            questBitsNightmare = new Label[5, 6, 16];
            for (int i = 0; i < 5; i++)
            {
                actlabelsNightmare[i] = new Label();
                actlabelsNightmare[i].Text = "Act " + (i + 1);
                actlabelsNightmare[i].SetBounds(20, i * 100, 40, 16);
                this.tabPage2.Controls.Add(actlabelsNightmare[i]);
                for (int j = 0; j < (i == 3 ? 3 : 6); j++)
                {
                    questlabelsNightmare[i, j] = new Label();
                    questlabelsNightmare[i, j].Text = "Quest " + (j + 1);
                    questlabelsNightmare[i, j].SetBounds(100, i * 100 + (j * 16), 80, 16);
                    this.tabPage2.Controls.Add(questlabelsNightmare[i, j]);
                    for (int k = 0; k < 16; k++)
                    {
                        questBitsNightmare[i, j, k] = new Label();
                        questBitsNightmare[i, j, k].Text = "" + k;
                        questBitsNightmare[i, j, k].Font = new Font(Font.SystemFontName, 6.5f);
                        questBitsNightmare[i, j, k].SetBounds(100 + 80 + (k) * 24, i * 100 + (j * 16), 20, 14);
                        questBitsNightmare[i, j, k].BorderStyle = BorderStyle.FixedSingle;
                        questBitsNightmare[i, j, k].BackColor = Color.LightGray;
                        this.tabPage2.Controls.Add(questBitsNightmare[i, j, k]);
                    }
                }
            }
            actlabelsHell = new Label[5];
            questlabelsHell = new Label[5, 6];
            questBitsHell = new Label[5, 6, 16];
            for (int i = 0; i < 5; i++)
            {
                actlabelsHell[i] = new Label();
                actlabelsHell[i].Text = "Act " + (i + 1);
                actlabelsHell[i].SetBounds(20, i * 100, 40, 16);
                this.tabPage3.Controls.Add(actlabelsHell[i]);
                for (int j = 0; j < (i == 3 ? 3 : 6); j++)
                {
                    questlabelsHell[i, j] = new Label();
                    questlabelsHell[i, j].Text = "Quest " + (j + 1);
                    questlabelsHell[i, j].SetBounds(100, i * 100 + (j * 16), 80, 16);
                    this.tabPage3.Controls.Add(questlabelsHell[i, j]);
                    for (int k = 0; k < 16; k++)
                    {
                        questBitsHell[i, j, k] = new Label();
                        questBitsHell[i, j, k].Text = "" + k;
                        questBitsHell[i, j, k].Font = new Font(Font.SystemFontName, 6.5f);
                        questBitsHell[i, j, k].SetBounds(100 + 80 + (k) * 24, i * 100 + (j * 16), 20, 14);
                        questBitsHell[i, j, k].BorderStyle = BorderStyle.FixedSingle;
                        questBitsHell[i, j, k].BackColor = Color.LightGray;
                        this.tabPage3.Controls.Add(questBitsHell[i, j, k]);
                    }
                }
            }
        }

        private void DebugWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
            }

        }
    }
}
