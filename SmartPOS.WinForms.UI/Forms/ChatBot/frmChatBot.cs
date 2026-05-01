using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.BLL.Services;
using SmartPOS.WinForms.DTO.Responses;
using SmartPOS.WinForms.UI.Interfaces;

namespace SmartPOS.WinForms.UI.Forms.ChatBot
{
    public class frmChatBot : Form, IGlobalSearchHandler
    {
        private readonly IChatBotService _chatBotService;

        private Label lblTitle;
        private Label lblSubtitle;
        private CardPanel pnlChat;
        private CardPanel pnlSide;
        private FlowLayoutPanel flpMessages;
        private TextBox txtQuestion;
        private Button btnSend;
        private FlowLayoutPanel flpSuggestions;
        private FlowLayoutPanel flpQuickQuestions;
        private Label lblStatus;

        private static readonly Color Bg = Color.FromArgb(248, 249, 251);
        private static readonly Color Surface = Color.White;
        private static readonly Color Primary = Color.FromArgb(22, 32, 72);
        private static readonly Color Accent = Color.FromArgb(90, 110, 200);
        private static readonly Color Border = Color.FromArgb(228, 231, 238);
        private static readonly Color Muted = Color.FromArgb(120, 132, 160);
        private static readonly Color Soft = Color.FromArgb(245, 247, 252);

        public frmChatBot()
        {
            _chatBotService = new ChatBotService();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "Trợ lý AI";
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Bg;
            Font = new Font("Segoe UI", 9F);
            Dock = DockStyle.Fill;

            lblTitle = new Label
            {
                Text = "Trợ lý AI SmartPOS",
                Font = new Font("Segoe UI Semibold", 16F, FontStyle.Bold),
                ForeColor = Primary,
                AutoSize = true,
                Location = new Point(20, 18)
            };

            lblSubtitle = new Label
            {
                Text = "Tra cứu doanh thu, tồn kho, hóa đơn, khách hàng và gợi ý vận hành từ dữ liệu hiện tại.",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Muted,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                Location = new Point(22, 47)
            };

            BuildChatPanel();
            BuildSidePanel();

            Controls.Add(lblTitle);
            Controls.Add(lblSubtitle);
            Controls.Add(pnlChat);
            Controls.Add(pnlSide);

            Load += FrmChatBot_Load;
            Resize += (s, e) => UpdateResponsiveLayout();
        }

        private void BuildChatPanel()
        {
            pnlChat = new CardPanel
            {
                BackColor = Surface,
                Radius = 8,
                BorderColor = Border
            };

            var lblChatTitle = new Label
            {
                Text = "Hội thoại",
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                ForeColor = Primary,
                AutoSize = true,
                Location = new Point(18, 16)
            };

            lblStatus = new Label
            {
                Text = "Bot nội bộ - truy vấn trực tiếp database",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Muted,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleRight,
                Size = new Size(290, 22),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            flpMessages = new FlowLayoutPanel
            {
                Location = new Point(16, 54),
                BackColor = Surface,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(0, 4, 0, 4)
            };
            flpMessages.HorizontalScroll.Enabled = false;

            txtQuestion = new TextBox
            {
                Location = new Point(16, 0),
                Multiline = true,
                Font = new Font("Segoe UI", 10F),
                BorderStyle = BorderStyle.FixedSingle
            };
            txtQuestion.KeyDown += TxtQuestion_KeyDown;

            btnSend = new Button
            {
                Text = "Gửi",
                BackColor = Primary,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSend.FlatAppearance.BorderSize = 0;
            btnSend.Click += (s, e) => SubmitQuestion(txtQuestion.Text);

            pnlChat.Controls.Add(lblChatTitle);
            pnlChat.Controls.Add(lblStatus);
            pnlChat.Controls.Add(flpMessages);
            pnlChat.Controls.Add(txtQuestion);
            pnlChat.Controls.Add(btnSend);
        }

        private void BuildSidePanel()
        {
            pnlSide = new CardPanel
            {
                BackColor = Surface,
                Radius = 8,
                BorderColor = Border
            };

            var lblQuickTitle = new Label
            {
                Text = "Câu hỏi nhanh",
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                ForeColor = Primary,
                AutoSize = true,
                Location = new Point(18, 16)
            };

            flpQuickQuestions = new FlowLayoutPanel
            {
                Location = new Point(18, 52),
                Size = new Size(300, 266),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Surface
            };

            string[] quickQuestions =
            {
                "Doanh thu hôm nay?",
                "Sản phẩm nào sắp hết hàng?",
                "Top 5 sản phẩm bán chạy?",
                "Hóa đơn mới nhất?",
                "Tổng số khách hàng?",
                "Có sản phẩm tồn kho cao cần khuyến mãi không?",
                "Sản phẩm nào nên nhập thêm?",
                "Hướng dẫn sử dụng POS"
            };

            foreach (string question in quickQuestions)
            {
                flpQuickQuestions.Controls.Add(MakeQuickButton(question));
            }

            var lblScopeTitle = new Label
            {
                Text = "Bot hỗ trợ",
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = Primary,
                AutoSize = true,
                Location = new Point(18, 334)
            };

            var lblScope = new Label
            {
                Text = "• Nhân viên bán hàng: tra cứu POS, hóa đơn, tồn kho\r\n" +
                       "• Quản lý: xem doanh thu, bán chạy, hàng tồn\r\n" +
                       "• Hướng dẫn sử dụng nhanh các chức năng POS\r\n" +
                       "• Gợi ý nhập hàng và khuyến mãi từ dữ liệu bán",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(70, 82, 120),
                AutoSize = false,
                Size = new Size(305, 112),
                Location = new Point(18, 366)
            };

            var lblSuggestTitle = new Label
            {
                Text = "Gợi ý tiếp theo",
                Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold),
                ForeColor = Primary,
                AutoSize = true,
                Location = new Point(18, 500)
            };

            flpSuggestions = new FlowLayoutPanel
            {
                Location = new Point(18, 532),
                Size = new Size(300, 150),
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Surface
            };

            pnlSide.Controls.Add(lblQuickTitle);
            pnlSide.Controls.Add(flpQuickQuestions);
            pnlSide.Controls.Add(lblScopeTitle);
            pnlSide.Controls.Add(lblScope);
            pnlSide.Controls.Add(lblSuggestTitle);
            pnlSide.Controls.Add(flpSuggestions);
        }

        private void FrmChatBot_Load(object sender, EventArgs e)
        {
            UpdateResponsiveLayout();
            AddBotMessage("Chào bạn. Mình có thể tra cứu doanh thu, tồn kho, hóa đơn, khách hàng và đưa ra gợi ý nhập hàng/khuyến mãi.");
            RenderSuggestions(new[]
            {
                "Doanh thu hôm nay?",
                "Sản phẩm nào sắp hết hàng?",
                "Top 5 sản phẩm bán chạy?"
            });
        }

        private Button MakeQuickButton(string text)
        {
            var button = new Button
            {
                Text = text,
                Size = new Size(292, 32),
                Margin = new Padding(0, 0, 0, 8),
                BackColor = Soft,
                ForeColor = Primary,
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 8.5F),
                Cursor = Cursors.Hand
            };
            button.FlatAppearance.BorderColor = Border;
            button.FlatAppearance.BorderSize = 1;
            button.Click += (s, e) => SubmitQuestion(text);
            return button;
        }

        private void SubmitQuestion(string question)
        {
            if (string.IsNullOrWhiteSpace(question))
            {
                return;
            }

            question = question.Trim();
            AddUserMessage(question);
            txtQuestion.Clear();

            Cursor previousCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            btnSend.Enabled = false;

            try
            {
                ChatBotResponse response = _chatBotService.Ask(question);
                AddBotMessage(response.Answer);
                RenderSuggestions(response.SuggestedQuestions);
            }
            finally
            {
                btnSend.Enabled = true;
                Cursor.Current = previousCursor;
            }
        }

        private void AddUserMessage(string text)
        {
            AddMessageBubble(text, true);
        }

        private void AddBotMessage(string text)
        {
            AddMessageBubble(text, false);
        }

        private void AddMessageBubble(string text, bool isUser)
        {
            int width = Math.Max(280, flpMessages.ClientSize.Width - 28);
            int bubbleWidth = Math.Min(620, Math.Max(260, width - 80));

            var wrapper = new Panel
            {
                Width = width,
                AutoSize = false,
                Margin = new Padding(0, 0, 0, 10),
                BackColor = Surface
            };

            var bubble = new BubblePanel
            {
                BackColor = isUser ? Primary : Soft,
                ForeColor = isUser ? Color.White : Color.FromArgb(40, 50, 80),
                Radius = 10,
                Padding = new Padding(12, 10, 12, 10),
                Width = bubbleWidth
            };

            var label = new Label
            {
                Text = text,
                AutoSize = false,
                MaximumSize = new Size(bubbleWidth - 24, 0),
                Font = new Font("Segoe UI", 9.2F),
                ForeColor = bubble.ForeColor,
                BackColor = Color.Transparent,
                Location = new Point(12, 10)
            };
            Size preferred = TextRenderer.MeasureText(text, label.Font, new Size(bubbleWidth - 24, 0), TextFormatFlags.WordBreak);
            label.Size = new Size(bubbleWidth - 24, Math.Max(22, preferred.Height + 4));

            bubble.Height = label.Height + 20;
            bubble.Controls.Add(label);
            bubble.Left = isUser ? width - bubbleWidth - 8 : 8;
            bubble.Top = 0;

            wrapper.Height = bubble.Height;
            wrapper.Controls.Add(bubble);
            flpMessages.Controls.Add(wrapper);
            flpMessages.ScrollControlIntoView(wrapper);
        }

        private void RenderSuggestions(IEnumerable<string> suggestions)
        {
            flpSuggestions.Controls.Clear();
            foreach (string suggestion in (suggestions ?? Enumerable.Empty<string>()).Take(4))
            {
                flpSuggestions.Controls.Add(MakeQuickButton(suggestion));
            }
        }

        private void TxtQuestion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || e.Shift)
            {
                return;
            }

            e.SuppressKeyPress = true;
            SubmitQuestion(txtQuestion.Text);
        }

        public void ApplyGlobalSearch(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return;
            }

            SubmitQuestion(keyword);
        }

        public void ClearGlobalSearch()
        {
            txtQuestion.Clear();
        }

        private void UpdateResponsiveLayout()
        {
            int pad = 20;
            int top = 84;
            int sideWidth = ClientSize.Width >= 1000 ? 350 : 0;
            int gap = sideWidth > 0 ? 14 : 0;
            int chatWidth = Math.Max(420, ClientSize.Width - (pad * 2) - sideWidth - gap);
            int height = Math.Max(360, ClientSize.Height - top - 20);

            lblSubtitle.SetBounds(22, 47, Math.Max(320, ClientSize.Width - 44), 22);
            pnlChat.SetBounds(pad, top, chatWidth, height);
            pnlSide.Visible = sideWidth > 0;
            if (pnlSide.Visible)
            {
                pnlSide.SetBounds(pnlChat.Right + gap, top, sideWidth, height);
            }

            lblStatus.Left = pnlChat.Width - lblStatus.Width - 18;
            lblStatus.Top = 16;

            int inputHeight = 54;
            btnSend.SetBounds(pnlChat.Width - 94, pnlChat.Height - inputHeight - 16, 76, inputHeight);
            txtQuestion.SetBounds(16, pnlChat.Height - inputHeight - 16, btnSend.Left - 28, inputHeight);
            flpMessages.SetBounds(16, 54, pnlChat.Width - 32, txtQuestion.Top - 66);

            foreach (Control wrapper in flpMessages.Controls)
            {
                wrapper.Width = Math.Max(280, flpMessages.ClientSize.Width - 28);
            }
        }

        private class CardPanel : Panel
        {
            public int Radius { get; set; } = 8;

            public Color BorderColor { get; set; } = Border;

            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
                using (GraphicsPath path = RoundedPath(rect, Radius))
                using (SolidBrush brush = new SolidBrush(BackColor))
                using (Pen pen = new Pen(BorderColor))
                {
                    e.Graphics.FillPath(brush, path);
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }

        private class BubblePanel : Panel
        {
            public int Radius { get; set; } = 10;

            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
                using (GraphicsPath path = RoundedPath(rect, Radius))
                using (SolidBrush brush = new SolidBrush(BackColor))
                {
                    e.Graphics.FillPath(brush, path);
                }
            }
        }

        private static GraphicsPath RoundedPath(Rectangle r, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
