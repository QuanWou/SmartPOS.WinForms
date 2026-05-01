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
        private CardPanel pnlInputWrap;
        private FlowLayoutPanel flpMessages;
        private TextBox txtQuestion;
        private Button btnSend;
        private FlowLayoutPanel flpSuggestions;
        private FlowLayoutPanel flpQuickQuestions;
        private Label lblStatus;

        private static readonly Color Bg = Color.FromArgb(248, 250, 252);
        private static readonly Color Surface = Color.White;
        private static readonly Color Primary = Color.FromArgb(22, 32, 72);
        private static readonly Color Accent = Color.FromArgb(43, 122, 241);
        private static readonly Color AccentLight = Color.FromArgb(70, 160, 255);
        private static readonly Color Border = Color.FromArgb(226, 232, 240);
        private static readonly Color Muted = Color.FromArgb(120, 132, 160);
        private static readonly Color Soft = Color.FromArgb(246, 248, 252);
        private static readonly Color ChatBg = Color.FromArgb(250, 252, 255);
        private static readonly Color TextMain = Color.FromArgb(30, 41, 59);
        private static readonly Color Online = Color.FromArgb(34, 197, 94);

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
                Radius = 14,
                BorderColor = Border
            };

            var botAvatar = new BotAvatar
            {
                Size = new Size(40, 40),
                Location = new Point(18, 14)
            };

            var lblChatTitle = new Label
            {
                Text = "Hội thoại AI",
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                ForeColor = Primary,
                AutoSize = true,
                Location = new Point(68, 15)
            };

            var lblChatSub = new Label
            {
                Text = "Tra cứu và phân tích dữ liệu bán hàng",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Muted,
                AutoSize = true,
                Location = new Point(70, 39)
            };

            lblStatus = new Label
            {
                Text = "● Đang online · Truy vấn database",
                Font = new Font("Segoe UI", 8.3F),
                ForeColor = Color.FromArgb(22, 101, 52),
                BackColor = Color.FromArgb(240, 253, 244),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(220, 24),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            flpMessages = new FlowLayoutPanel
            {
                Location = new Point(16, 66),
                BackColor = ChatBg,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(10, 10, 10, 10)
            };
            flpMessages.HorizontalScroll.Enabled = false;

            pnlInputWrap = new CardPanel
            {
                BackColor = Color.FromArgb(249, 250, 252),
                Radius = 14,
                BorderColor = Border
            };

            txtQuestion = new TextBox
            {
                Multiline = true,
                Font = new Font("Segoe UI", 9.5F),
                BorderStyle = BorderStyle.None,
                ForeColor = Primary,
                BackColor = Color.FromArgb(249, 250, 252)
            };
            txtQuestion.KeyDown += TxtQuestion_KeyDown;

            btnSend = new SendButton
            {
                Text = "➜",
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSend.Click += (s, e) => SubmitQuestion(txtQuestion.Text);

            pnlInputWrap.Controls.Add(txtQuestion);
            pnlInputWrap.Controls.Add(btnSend);

            pnlChat.Controls.Add(botAvatar);
            pnlChat.Controls.Add(lblChatTitle);
            pnlChat.Controls.Add(lblChatSub);
            pnlChat.Controls.Add(lblStatus);
            pnlChat.Controls.Add(flpMessages);
            pnlChat.Controls.Add(pnlInputWrap);
        }

        private void BuildSidePanel()
        {
            pnlSide = new CardPanel
            {
                BackColor = Surface,
                Radius = 14,
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
                Location = new Point(18, 318)
            };

            var lblScope = new Label
            {
                Text = "• Tra cứu doanh thu, hóa đơn, tồn kho\r\n" +
                       "• Xem sản phẩm bán chạy, hàng tồn\r\n" +
                       "• Gợi ý nhập hàng và khuyến mãi\r\n" +
                       "• Hướng dẫn nhanh chức năng POS",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(70, 82, 120),
                AutoSize = false,
                Size = new Size(305, 100),
                Location = new Point(18, 348)
            }; 

           

            pnlSide.Controls.Add(lblQuickTitle);
            pnlSide.Controls.Add(flpQuickQuestions);
            pnlSide.Controls.Add(lblScopeTitle);
            pnlSide.Controls.Add(lblScope);
           
        }

        private void FrmChatBot_Load(object sender, EventArgs e)
        {
            UpdateResponsiveLayout();

            AddBotMessage(
                "Chào bạn. Mình có thể tra cứu doanh thu, tồn kho, hóa đơn, khách hàng " +
                "và đưa ra gợi ý nhập hàng/khuyến mãi.");
        }

        private Button MakeQuickButton(string text)
        {
            var button = new QuickButton
            {
                Text = text,
                Size = new Size(292, 36),
                Margin = new Padding(0, 0, 0, 8),
                Font = new Font("Segoe UI Semibold", 8.6F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

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
            int rowWidth = Math.Max(260, flpMessages.ClientSize.Width - 24);

            Font messageFont = new Font("Segoe UI", 8.8F);

            int maxBubbleWidth = isUser
                ? Math.Min(360, rowWidth - 140)
                : Math.Min(520, rowWidth - 90);

            int minBubbleWidth = isUser ? 170 : 220;

            Size measured = TextRenderer.MeasureText(
                text,
                messageFont,
                new Size(maxBubbleWidth - 24, 0),
                TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix);

            int bubbleWidth = Math.Max(minBubbleWidth, Math.Min(maxBubbleWidth, measured.Width + 30));
            int bubbleHeight = Math.Max(48, measured.Height + 34);

            var wrapper = new Panel
            {
                Width = rowWidth,
                Height = bubbleHeight,
                AutoSize = false,
                Margin = new Padding(0, 0, 0, 10),
                BackColor = ChatBg,
                Tag = isUser
            };

            var bubble = new ChatBubbleControl
            {
                MessageText = text,
                IsUserMessage = isUser,
                CreatedAt = DateTime.Now,
                Width = bubbleWidth,
                Height = bubbleHeight,
                Font = messageFont
            };

            bubble.Left = isUser ? rowWidth - bubbleWidth - 6 : 6;
            bubble.Top = 0;

            wrapper.Controls.Add(bubble);
            flpMessages.Controls.Add(wrapper);
            flpMessages.ScrollControlIntoView(wrapper);
        }

        private void RenderSuggestions(IEnumerable<string> suggestions)
        {
            var list = (suggestions ?? Enumerable.Empty<string>())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Take(6)
                .ToList();

            if (list.Count == 0)
            {
                return;
            }

            flpQuickQuestions.Controls.Clear();

            foreach (string suggestion in list)
            {
                flpQuickQuestions.Controls.Add(MakeQuickButton(suggestion));
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
            lblStatus.Top = 20;

            int inputHeight = 58;
            pnlInputWrap.SetBounds(18, pnlChat.Height - inputHeight - 18, pnlChat.Width - 36, inputHeight);

            btnSend.SetBounds(pnlInputWrap.Width - 50, 9, 40, 40);
            txtQuestion.SetBounds(14, 12, pnlInputWrap.Width - 76, 34);

            flpMessages.SetBounds(18, 66, pnlChat.Width - 36, pnlInputWrap.Top - 78);

            foreach (Control wrapper in flpMessages.Controls)
            {
                wrapper.Width = Math.Max(260, flpMessages.ClientSize.Width - 24);

                if (wrapper.Controls.Count > 0)
                {
                    Control bubble = wrapper.Controls[0];

                    bool isUser = wrapper.Tag is bool value && value;

                    bubble.Left = isUser
                        ? wrapper.Width - bubble.Width - 6
                        : 6;
                }
            }

            if (pnlSide.Visible)
            {
                flpQuickQuestions.SetBounds(18, 52, pnlSide.Width - 36, 266);

                foreach (Control control in flpQuickQuestions.Controls)
                {
                    control.Width = flpQuickQuestions.Width - 4;
                }
            }
        }

        private class CardPanel : Panel
        {
            public int Radius { get; set; } = 14;

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
        private class ChatBubbleControl : Control
        {
            public string MessageText { get; set; } = string.Empty;

            public bool IsUserMessage { get; set; }

            public DateTime CreatedAt { get; set; } = DateTime.Now;

            public ChatBubbleControl()
            {
                BackColor = ChatBg;

                SetStyle(
                    ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.UserPaint |
                    ControlStyles.OptimizedDoubleBuffer |
                    ControlStyles.ResizeRedraw,
                    true);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);

                Color bubbleColor = IsUserMessage ? Accent : Surface;
                Color borderColor = IsUserMessage ? Accent : Border;
                Color textColor = IsUserMessage ? Color.White : TextMain;
                Color timeColor = IsUserMessage ? Color.FromArgb(225, 235, 255) : Muted;

                using (GraphicsPath path = RoundedPath(rect, 14))
                using (SolidBrush brush = new SolidBrush(bubbleColor))
                using (Pen pen = new Pen(borderColor))
                {
                    e.Graphics.FillPath(brush, path);

                    if (!IsUserMessage)
                    {
                        e.Graphics.DrawPath(pen, path);
                    }
                }

                Rectangle textRect = new Rectangle(12, 9, Width - 24, Height - 28);

                TextRenderer.DrawText(
                    e.Graphics,
                    MessageText,
                    Font,
                    textRect,
                    textColor,
                    TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix);

                Rectangle timeRect = new Rectangle(12, Height - 18, Width - 24, 14);

                TextRenderer.DrawText(
                    e.Graphics,
                    CreatedAt.ToString("HH:mm"),
                    new Font("Segoe UI", 6.8F),
                    timeRect,
                    timeColor,
                    TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
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
        private class SendButton : Button
        {
            private bool _hover;

            public SendButton()
            {
                TextAlign = ContentAlignment.MiddleCenter;
                FlatStyle = FlatStyle.Flat;
                 BackColor = Color.FromArgb(249, 250, 252);
                ForeColor = Color.White;
                TabStop = false;

                FlatAppearance.BorderSize = 0;
                FlatAppearance.MouseDownBackColor = Color.FromArgb(249, 250, 252);
                FlatAppearance.MouseOverBackColor = Color.FromArgb(249, 250, 252);

                SetStyle(
                    ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.UserPaint |
                    ControlStyles.OptimizedDoubleBuffer |
                    ControlStyles.ResizeRedraw |
                    ControlStyles.SupportsTransparentBackColor,
                    true);
            }

            protected override void OnMouseEnter(EventArgs e)
            {
                _hover = true;
                Invalidate();
                base.OnMouseEnter(e);
            }

            protected override void OnMouseLeave(EventArgs e)
            {
                _hover = false;
                Invalidate();
                base.OnMouseLeave(e);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);

                using (LinearGradientBrush brush = new LinearGradientBrush(
                    rect,
                    _hover ? AccentLight : Accent,
                    Primary,
                    LinearGradientMode.ForwardDiagonal))
                {
                    e.Graphics.FillEllipse(brush, rect);
                }

                TextRenderer.DrawText(
                    e.Graphics,
                    Text,
                    Font,
                    rect,
                    Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }

        private class QuickButton : Button
        {
            private bool _hover;

            public QuickButton()
            {
                TextAlign = ContentAlignment.MiddleLeft;
                FlatStyle = FlatStyle.Flat;
                BackColor = Surface;
                ForeColor = Primary;
                TabStop = false;

                FlatAppearance.BorderSize = 0;
                FlatAppearance.MouseDownBackColor = Soft;
                FlatAppearance.MouseOverBackColor = Soft;

                SetStyle(
                    ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.UserPaint |
                    ControlStyles.OptimizedDoubleBuffer |
                    ControlStyles.ResizeRedraw,
                    true);
            }

            protected override void OnMouseEnter(EventArgs e)
            {
                _hover = true;
                Invalidate();
                base.OnMouseEnter(e);
            }

            protected override void OnMouseLeave(EventArgs e)
            {
                _hover = false;
                Invalidate();
                base.OnMouseLeave(e);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);

                using (GraphicsPath path = RoundedPath(rect, 12))
                using (SolidBrush brush = new SolidBrush(_hover ? Color.FromArgb(239, 246, 255) : Soft))
                using (Pen pen = new Pen(_hover ? Accent : Border))
                {
                    e.Graphics.FillPath(brush, path);
                    e.Graphics.DrawPath(pen, path);
                }

                TextRenderer.DrawText(
                    e.Graphics,
                    Text,
                    Font,
                    new Rectangle(12, 0, Width - 20, Height),
                    ForeColor,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
            }
        }

        private class BotAvatar : Control
        {
            public BotAvatar()
            {
                BackColor = Surface;

                SetStyle(
                    ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.UserPaint |
                    ControlStyles.OptimizedDoubleBuffer |
                    ControlStyles.ResizeRedraw |
                    ControlStyles.SupportsTransparentBackColor,
                    true);
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.CompositingQuality = CompositingQuality.HighQuality;

                Rectangle circle = new Rectangle(1, 1, Width - 3, Height - 3);

                using (LinearGradientBrush brush = new LinearGradientBrush(
                    circle,
                    AccentLight,
                    Primary,
                    LinearGradientMode.ForwardDiagonal))
                {
                    e.Graphics.FillEllipse(brush, circle);
                }

                using (Pen pen = new Pen(Color.FromArgb(90, Color.White), 1))
                {
                    e.Graphics.DrawEllipse(pen, circle);
                }

                DrawRobot(e.Graphics, circle);
            }

            private void DrawRobot(Graphics g, Rectangle bounds)
            {
                float centerX = bounds.X + bounds.Width / 2f;

                float faceW = bounds.Width * 0.58f;
                float faceH = bounds.Height * 0.42f;
                float faceX = centerX - faceW / 2f;
                float faceY = bounds.Y + bounds.Height * 0.38f;

                RectangleF faceRect = new RectangleF(faceX, faceY, faceW, faceH);

                using (Pen antennaPen = new Pen(Color.White, 2f))
                {
                    antennaPen.StartCap = LineCap.Round;
                    antennaPen.EndCap = LineCap.Round;

                    float antTop = bounds.Y + bounds.Height * 0.20f;
                    float antBase = faceRect.Y - 1;

                    g.DrawLine(antennaPen, centerX, antBase, centerX, antTop + 4);
                    g.DrawLine(antennaPen, centerX, antTop + 4, centerX - 5, antTop);
                    g.DrawLine(antennaPen, centerX, antTop + 4, centerX + 5, antTop);
                }

                using (SolidBrush whiteBrush = new SolidBrush(Color.FromArgb(245, 248, 255)))
                {
                    g.FillEllipse(whiteBrush, centerX - 2.5f, bounds.Y + bounds.Height * 0.18f - 2.5f, 5, 5);

                    float earW = faceW * 0.16f;
                    float earH = faceH * 0.42f;
                    float earY = faceRect.Y + faceH * 0.33f;

                    g.FillEllipse(whiteBrush, faceRect.X - earW * 0.55f, earY, earW, earH);
                    g.FillEllipse(whiteBrush, faceRect.Right - earW * 0.45f, earY, earW, earH);

                    using (GraphicsPath facePath = RoundedRectF(faceRect, faceH / 2f))
                    {
                        g.FillPath(whiteBrush, facePath);
                    }
                }

                float darkW = faceW * 0.78f;
                float darkH = faceH * 0.48f;

                RectangleF darkRect = new RectangleF(
                    centerX - darkW / 2f,
                    faceRect.Y + faceH * 0.25f,
                    darkW,
                    darkH);

                using (GraphicsPath darkPath = RoundedRectF(darkRect, darkH / 2f))
                using (SolidBrush darkBrush = new SolidBrush(Color.FromArgb(20, 40, 70)))
                {
                    g.FillPath(darkBrush, darkPath);
                }

                using (SolidBrush eyeBrush = new SolidBrush(Color.FromArgb(0, 230, 255)))
                {
                    float eyeW = darkRect.Width * 0.18f;
                    float eyeH = darkRect.Height * 0.42f;
                    float eyeY = darkRect.Y + (darkRect.Height - eyeH) / 2f;

                    g.FillEllipse(eyeBrush, darkRect.X + darkRect.Width * 0.25f, eyeY, eyeW, eyeH);
                    g.FillEllipse(eyeBrush, darkRect.Right - darkRect.Width * 0.25f - eyeW, eyeY, eyeW, eyeH);
                }
            }
        }

        private static GraphicsPath RoundedRectF(RectangleF rect, float radius)
        {
            float d = radius * 2f;

            if (d > rect.Width)
            {
                d = rect.Width;
            }

            if (d > rect.Height)
            {
                d = rect.Height;
            }

            GraphicsPath path = new GraphicsPath();

            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();

            return path;
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
