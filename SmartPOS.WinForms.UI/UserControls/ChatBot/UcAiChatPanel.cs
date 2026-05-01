using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using SmartPOS.WinForms.BLL.Interfaces;
using SmartPOS.WinForms.BLL.Services;
using SmartPOS.WinForms.Common.Session;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.DTO.Responses;

namespace SmartPOS.WinForms.UI.UserControls.ChatBot
{
    public class UcAiChatPanel : UserControl
    {
        private readonly IChatBotService _chatBotService;

        private Panel pnlHeader;
        private FlowLayoutPanel flpMessages;
        private FlowLayoutPanel flpQuickActions;
        private TextBox txtQuestion;
        private Button btnSend;
        private Button btnClose;
        private Button btnExpand;
        private Label lblStatus;

        private static readonly Color Surface = Color.White;
        private static readonly Color Primary = Color.FromArgb(22, 32, 72);
        private static readonly Color AccentBlue = Color.FromArgb(43, 122, 241);
        private static readonly Color AccentLight = Color.FromArgb(70, 160, 255);
        private static readonly Color Border = Color.FromArgb(228, 231, 238);
        private static readonly Color Muted = Color.FromArgb(120, 132, 160);
        private static readonly Color Soft = Color.FromArgb(246, 248, 252);
        private static readonly Color ChatBg = Color.FromArgb(250, 252, 255);
        private static readonly Color TextMain = Color.FromArgb(30, 41, 59);
        private static readonly Color Online = Color.FromArgb(34, 197, 94);

        public event EventHandler CloseRequested;

        public event EventHandler ExpandRequested;

        public UcAiChatPanel()
        {
            _chatBotService = new ChatBotService();
            Width = 360;
            MinimumSize = new Size(320, 360);
            BackColor = Surface;
            DoubleBuffered = true;

            BuildLayout();
            Load += UcAiChatPanel_Load;
            Resize += (s, e) => LayoutChildren();
        }

       
        private void BuildLayout()
        {
            pnlHeader = new Panel
            {
                BackColor = Surface,
                Height = 78,
                Dock = DockStyle.Top
            };
            pnlHeader.Paint += Header_Paint;

            var botAvatar = new BotAvatar
            {
                Size = new Size(44, 44),
                Location = new Point(16, 17)
            };

            var lblTitle = new Label
            {
                Text = "SmartPOS AI",
                Font = new Font("Segoe UI Semibold", 12.2F, FontStyle.Bold),
                ForeColor = Primary,
                AutoSize = true,
                Location = new Point(70, 18)
            };

            var lblSub = new Label
            {
                Text = "Bot phân tích & gợi ý vận hành",
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = Muted,
                AutoSize = true,
                Location = new Point(72, 43)
            };

            btnExpand = BuildHeaderButton("↗");
            btnExpand.Click += (s, e) => ExpandRequested?.Invoke(this, EventArgs.Empty);

            btnClose = BuildHeaderButton("×");
            btnClose.Click += (s, e) => CloseRequested?.Invoke(this, EventArgs.Empty);

            pnlHeader.Controls.Add(botAvatar);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSub);
            pnlHeader.Controls.Add(btnExpand);
            pnlHeader.Controls.Add(btnClose);

            lblStatus = new Label
            {
                Text = "  ●  Đang online · Theo dõi dữ liệu bán hàng",
                Font = new Font("Segoe UI", 8.2F),
                ForeColor = Color.FromArgb(22, 101, 52),
                BackColor = Color.FromArgb(240, 253, 244),
                AutoSize = false,
                Height = 30,
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0)
            };

            flpMessages = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = ChatBg,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(10, 8, 10, 8)
            };
            flpMessages.HorizontalScroll.Enabled = false;

            flpQuickActions = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 86,
                BackColor = Surface,
                Padding = new Padding(14, 9, 14, 5),
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight
            };
            AddQuickActions();

            var pnlInput = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 92,
                BackColor = Surface,
                Padding = new Padding(14, 8, 14, 10)
            };

            var inputWrap = new RoundedPanel
            {
                BackColor = Color.FromArgb(249, 250, 252),
                BorderColor = Border,
                Radius = 14
            };
            inputWrap.SetBounds(14, 8, Width - 28, 58);
            inputWrap.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

            txtQuestion = new TextBox
            {
                BorderStyle = BorderStyle.None,
                Multiline = true,
                Font = new Font("Segoe UI", 9.2F),
                ForeColor = Primary,
                BackColor = Color.FromArgb(249, 250, 252),
                Location = new Point(14, 12),
                Size = new Size(inputWrap.Width - 76, 34),
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right
            };
            txtQuestion.KeyDown += TxtQuestion_KeyDown;

            btnSend = new Button
            {
                Text = "➜",
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = AccentBlue,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Size = new Size(40, 40),
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
                Location = new Point(inputWrap.Width - 50, 9)
            };
            btnSend.FlatAppearance.BorderSize = 0;
            btnSend.FlatAppearance.MouseOverBackColor = Color.FromArgb(37, 99, 235);
            btnSend.FlatAppearance.MouseDownBackColor = Color.FromArgb(29, 78, 216);
            btnSend.Click += (s, e) => SubmitQuestion(txtQuestion.Text);

            inputWrap.Resize += (s, e) =>
            {
                btnSend.Left = inputWrap.Width - 50;
                txtQuestion.Width = inputWrap.Width - 76;
                ApplyRoundedRegion(btnSend, 20);
            };
            ApplyRoundedRegion(btnSend, 20);

            inputWrap.Controls.Add(txtQuestion);
            inputWrap.Controls.Add(btnSend);
            pnlInput.Controls.Add(inputWrap);

            var lblHint = new Label
            {
                Text = "AI có thể mắc sai sót. Hãy kiểm tra lại thông tin quan trọng.",
                Dock = DockStyle.Bottom,
                Height = 17,
                Font = new Font("Segoe UI", 7F),
                ForeColor = Color.FromArgb(148, 163, 184),
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlInput.Controls.Add(lblHint);

            Controls.Add(flpMessages);
            Controls.Add(flpQuickActions);
            Controls.Add(pnlInput);
            Controls.Add(lblStatus);
            Controls.Add(pnlHeader);
        }

        private void UcAiChatPanel_Load(object sender, EventArgs e)
        {
            EnsureWelcomeMessage();
            RenderMessages();
            LayoutChildren();
        }

        public void FocusInput()
        {
            txtQuestion.Focus();
        }

        public void Ask(string question)
        {
            SubmitQuestion(question);
        }

        public void ReloadFromSession()
        {
            EnsureWelcomeMessage();
            RenderMessages();
        }

        private void EnsureWelcomeMessage()
        {
            if (SessionManager.ChatMessages.Count > 0)
            {
                return;
            }

            SessionManager.AddChatMessage(false,
                "Xin chào! Tôi là trợ lý AI của SmartPOS.\r\n\r\n" +
                "Tôi có thể phân tích dữ liệu bán hàng và đưa ra gợi ý giúp bạn tối ưu hoạt động.");
        }

        private void AddQuickActions()
        {
            flpQuickActions.Controls.Clear();

            flpQuickActions.Controls.Add(BuildQuickActionButton("📈 Doanh thu", "Phân tích doanh thu tuần này so với tuần trước"));
            flpQuickActions.Controls.Add(BuildQuickActionButton("🛒 Nhập hàng", "Sản phẩm nào nên nhập thêm?"));
            flpQuickActions.Controls.Add(BuildQuickActionButton("🏆 Bán chạy", "Top 5 sản phẩm bán chạy?"));
            flpQuickActions.Controls.Add(BuildQuickActionButton("📦 Tồn kho", "Sản phẩm nào sắp hết hàng?"));
        }

        private Button BuildQuickActionButton(string text, string question)
        {
            var button = new QuickActionButton
            {
                Text = text,
                Width = 150,
                Height = 32,
                Margin = new Padding(0, 0, 8, 8),
                Font = new Font("Segoe UI Semibold", 8.2F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };

            button.Click += (s, e) => SubmitQuestion(question);
            return button;
        }

        private Button BuildHeaderButton(string text)
        {
            var button = new HeaderIconButton
            {
                Text = text,
                Size = new Size(32, 32),
                Font = new Font("Segoe UI", 12.5F),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            return button;
        }

        private void SubmitQuestion(string question)
        {
            if (string.IsNullOrWhiteSpace(question))
            {
                return;
            }

            question = question.Trim();
            txtQuestion.Clear();

            SessionManager.AddChatMessage(true, question);
            AddMessageBubble(SessionManager.ChatMessages.Last());
            ScrollToBottom();

            Cursor previousCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            btnSend.Enabled = false;

            try
            {
                ChatBotResponse response = _chatBotService.Ask(question);
                SessionManager.AddChatMessage(false, response.Answer);
                AddMessageBubble(SessionManager.ChatMessages.Last());
                ScrollToBottom();
            }
            finally
            {
                btnSend.Enabled = true;
                Cursor.Current = previousCursor;
                txtQuestion.Focus();
            }
        }

        private void RenderMessages()
        {
            flpMessages.SuspendLayout();
            flpMessages.Controls.Clear();
            foreach (ChatSessionMessageDTO message in SessionManager.ChatMessages)
            {
                AddMessageBubble(message);
            }
            flpMessages.ResumeLayout(true);
            ScrollToBottom();
        }

        private void AddMessageBubble(ChatSessionMessageDTO message)
        {
            bool isUser = message.IsUser;

            int rowWidth = Math.Max(240, flpMessages.ClientSize.Width - 20);

            int bubbleWidth;
            if (Width >= 400)
            {
                bubbleWidth = isUser ? 250 : 270;
            }
            else
            {
                bubbleWidth = isUser ? 220 : 240;
            }

            bubbleWidth = Math.Min(bubbleWidth, rowWidth - 16);
            bubbleWidth = Math.Max(150, bubbleWidth);

            var row = new Panel
            {
                Width = rowWidth,
                Height = 1,
                BackColor = flpMessages.BackColor,
                Margin = new Padding(0, 0, 0, 8)
            };

            var bubble = new BubblePanel
            {
                Radius = 12,
                BackColor = isUser ? AccentBlue : Surface,
                BorderColor = isUser ? AccentBlue : Border,
                Width = bubbleWidth
            };

            var text = new Label
            {
                Text = message.Text,
                AutoSize = false,
                Font = new Font("Segoe UI", 8.4F),
                ForeColor = isUser ? Color.White : TextMain,
                BackColor = Color.Transparent,
                Location = new Point(10, 8)
            };

            Size measured = TextRenderer.MeasureText(
                text.Text,
                text.Font,
                new Size(bubbleWidth - 20, 0),
                TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix);

            text.Size = new Size(bubbleWidth - 20, Math.Max(18, measured.Height));

            var time = new Label
            {
                Text = message.CreatedAt.ToString("HH:mm"),
                Font = new Font("Segoe UI", 6.8F),
                ForeColor = isUser ? Color.FromArgb(220, 230, 255) : Muted,
                BackColor = Color.Transparent,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleRight,
                Size = new Size(bubbleWidth - 20, 14),
                Location = new Point(10, text.Bottom + 2)
            };

            bubble.Height = time.Bottom + 8;
            bubble.Controls.Add(text);
            bubble.Controls.Add(time);

            bubble.Left = isUser ? rowWidth - bubbleWidth - 2 : 2;
            bubble.Top = 0;

            row.Height = bubble.Height;
            row.Controls.Add(bubble);

            flpMessages.Controls.Add(row);
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

        private void Header_Paint(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(Border))
            {
                e.Graphics.DrawLine(pen, 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);
            }
        }

        private void LayoutChildren()
        {
            btnClose.Location = new Point(Width - 44, 23);
            btnExpand.Location = new Point(Width - 80, 23);

            foreach (Control control in flpMessages.Controls)
            {
                control.Width = Math.Max(270, flpMessages.ClientSize.Width - 34);
            }

            int quickWidth = Width >= 400 ? 174 : 150;

            foreach (Control control in flpQuickActions.Controls)
            {
                control.Width = quickWidth;
            }
        }

        private void ScrollToBottom()
        {
            if (flpMessages.Controls.Count == 0)
            {
                return;
            }

            flpMessages.ScrollControlIntoView(flpMessages.Controls[flpMessages.Controls.Count - 1]);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (Pen pen = new Pen(Border))
            {
                e.Graphics.DrawLine(pen, 0, 0, 0, Height);
            }
        }

        private static void ApplyRoundedRegion(Control ctrl, int radius)
        {
            if (ctrl.Width <= 0 || ctrl.Height <= 0)
            {
                return;
            }

            using (GraphicsPath path = RoundedPath(new Rectangle(0, 0, ctrl.Width, ctrl.Height), radius))
            {
                ctrl.Region = new Region(path);
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

        private class BubblePanel : Panel
        {
            public int Radius { get; set; } = 10;

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

        private class RoundedPanel : Panel
        {
            public int Radius { get; set; } = 10;

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
        private class BotAvatar : Control
        {
            public BotAvatar()
            {
                SetStyle(
                    ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.UserPaint |
                    ControlStyles.OptimizedDoubleBuffer |
                    ControlStyles.ResizeRedraw |
                    ControlStyles.SupportsTransparentBackColor,
                    true);

                BackColor = Color.Transparent;
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

                Rectangle circle = new Rectangle(1, 1, Width - 3, Height - 3);

                using (LinearGradientBrush brush = new LinearGradientBrush(
                    circle,
                    AccentBlue,
                    Primary,
                    LinearGradientMode.ForwardDiagonal))
                {
                    e.Graphics.FillEllipse(brush, circle);
                }

                using (Pen borderPen = new Pen(Color.FromArgb(90, Color.White), 1))
                {
                    e.Graphics.DrawEllipse(borderPen, circle);
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

                // Antenna
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

                    // Ears
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

                // Eye area
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

                // Eyes
                using (SolidBrush eyeBrush = new SolidBrush(Color.FromArgb(0, 230, 255)))
                {
                    float eyeW = darkRect.Width * 0.18f;
                    float eyeH = darkRect.Height * 0.42f;
                    float eyeY = darkRect.Y + (darkRect.Height - eyeH) / 2f;

                    g.FillEllipse(eyeBrush, darkRect.X + darkRect.Width * 0.25f, eyeY, eyeW, eyeH);
                    g.FillEllipse(eyeBrush, darkRect.Right - darkRect.Width * 0.25f - eyeW, eyeY, eyeW, eyeH);
                }

                // Mouth
                using (Pen mouthPen = new Pen(Color.FromArgb(20, 40, 70), 1.5f))
                {
                    mouthPen.StartCap = LineCap.Round;
                    mouthPen.EndCap = LineCap.Round;

                    float mouthW = faceW * 0.15f;
                    float mouthY = faceRect.Y + faceH * 0.76f;

                    g.DrawLine(mouthPen, centerX - mouthW / 2f, mouthY, centerX + mouthW / 2f, mouthY);
                }
            }
        }

        private class HeaderIconButton : Button
        {
            private bool _hover;

            public HeaderIconButton()
            {
                TextAlign = ContentAlignment.MiddleCenter;
                FlatStyle = FlatStyle.Flat;
                BackColor = Surface;
                ForeColor = Primary;
                TabStop = false;

                FlatAppearance.BorderSize = 0;
                FlatAppearance.MouseDownBackColor = Color.Transparent;
                FlatAppearance.MouseOverBackColor = Color.Transparent;

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

                using (GraphicsPath path = RoundedPath(rect, 10))
                using (SolidBrush brush = new SolidBrush(_hover ? Soft : Surface))
                {
                    e.Graphics.FillPath(brush, path);
                }

                TextRenderer.DrawText(
                    e.Graphics,
                    Text,
                    Font,
                    rect,
                    ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }

        private class QuickActionButton : Button
        {
            private bool _hover;

            public QuickActionButton()
            {
                TextAlign = ContentAlignment.MiddleLeft;
                FlatStyle = FlatStyle.Flat;
                BackColor = Surface;
                ForeColor = Primary;
                TabStop = false;

                FlatAppearance.BorderSize = 0;
                FlatAppearance.MouseDownBackColor = Color.Transparent;
                FlatAppearance.MouseOverBackColor = Color.Transparent;

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
                using (SolidBrush brush = new SolidBrush(_hover ? Color.FromArgb(239, 246, 255) : Surface))
                using (Pen pen = new Pen(_hover ? AccentBlue : Border))
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
    }

}
