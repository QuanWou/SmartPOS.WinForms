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
        private static readonly Color Border = Color.FromArgb(228, 231, 238);
        private static readonly Color Muted = Color.FromArgb(120, 132, 160);
        private static readonly Color Soft = Color.FromArgb(246, 248, 252);

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
                Height = 84,
                Dock = DockStyle.Top
            };
            pnlHeader.Paint += Header_Paint;

            var lblBotIcon = new Label
            {
                Text = "AI",
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Primary,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(38, 38),
                Location = new Point(18, 22)
            };
            ApplyRoundedRegion(lblBotIcon, 19);

            var lblTitle = new Label
            {
                Text = "SmartPOS AI",
                Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold),
                ForeColor = Primary,
                AutoSize = true,
                Location = new Point(68, 21)
            };

            var lblSub = new Label
            {
                Text = "Bot phân tích & gợi ý",
                Font = new Font("Segoe UI", 8.6F),
                ForeColor = Muted,
                AutoSize = true,
                Location = new Point(70, 46)
            };

            btnExpand = BuildHeaderButton("↗");
            btnExpand.Click += (s, e) => ExpandRequested?.Invoke(this, EventArgs.Empty);

            btnClose = BuildHeaderButton("×");
            btnClose.Click += (s, e) => CloseRequested?.Invoke(this, EventArgs.Empty);

            pnlHeader.Controls.Add(lblBotIcon);
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSub);
            pnlHeader.Controls.Add(btnExpand);
            pnlHeader.Controls.Add(btnClose);

            lblStatus = new Label
            {
                Text = "  ●  Đang theo dõi dữ liệu bán hàng",
                Font = new Font("Segoe UI", 8.3F),
                ForeColor = Color.FromArgb(75, 90, 125),
                BackColor = Color.FromArgb(250, 251, 254),
                AutoSize = false,
                Height = 32,
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleLeft
            };

            flpMessages = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                BackColor = Surface,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(14, 12, 14, 12)
            };
            flpMessages.HorizontalScroll.Enabled = false;

            flpQuickActions = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 78,
                BackColor = Surface,
                Padding = new Padding(14, 8, 14, 4),
                WrapContents = true,
                FlowDirection = FlowDirection.LeftToRight
            };
            AddQuickActions();

            var pnlInput = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 88,
                BackColor = Surface,
                Padding = new Padding(14, 6, 14, 10)
            };

            var inputWrap = new RoundedPanel
            {
                BackColor = Surface,
                BorderColor = Border,
                Radius = 10
            };
            inputWrap.SetBounds(14, 6, Width - 28, 68);
            inputWrap.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Top;

            txtQuestion = new TextBox
            {
                BorderStyle = BorderStyle.None,
                Multiline = true,
                Font = new Font("Segoe UI", 9.2F),
                ForeColor = Primary,
                BackColor = Surface,
                Location = new Point(13, 12),
                Size = new Size(Width - 94, 42),
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right
            };
            txtQuestion.KeyDown += TxtQuestion_KeyDown;

            btnSend = new Button
            {
                Text = "➤",
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Primary,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Size = new Size(44, 44),
                Anchor = AnchorStyles.Right | AnchorStyles.Top,
                Location = new Point(inputWrap.Width - 56, 12)
            };
            btnSend.FlatAppearance.BorderSize = 0;
            btnSend.Click += (s, e) => SubmitQuestion(txtQuestion.Text);
            inputWrap.Resize += (s, e) =>
            {
                btnSend.Left = inputWrap.Width - 56;
                txtQuestion.Width = inputWrap.Width - 82;
                ApplyRoundedRegion(btnSend, 22);
            };
            ApplyRoundedRegion(btnSend, 22);

            inputWrap.Controls.Add(txtQuestion);
            inputWrap.Controls.Add(btnSend);
            pnlInput.Controls.Add(inputWrap);

            var lblHint = new Label
            {
                Text = "AI có thể mắc sai sót. Vui lòng kiểm tra lại thông tin quan trọng.",
                Dock = DockStyle.Bottom,
                Height = 15,
                Font = new Font("Segoe UI", 7F),
                ForeColor = Color.FromArgb(155, 163, 184),
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
            flpQuickActions.Controls.Add(BuildQuickActionButton("↗ Phân tích doanh thu", "Phân tích doanh thu tuần này so với tuần trước"));
            flpQuickActions.Controls.Add(BuildQuickActionButton("🧺 Gợi ý nhập hàng", "Sản phẩm nào nên nhập thêm?"));
            flpQuickActions.Controls.Add(BuildQuickActionButton("🏆 Top bán chạy", "Top 5 sản phẩm bán chạy?"));
            flpQuickActions.Controls.Add(BuildQuickActionButton("📦 Tồn kho thấp", "Sản phẩm nào sắp hết hàng?"));
        }

        private Button BuildQuickActionButton(string text, string question)
        {
            var button = new Button
            {
                Text = text,
                Width = 150,
                Height = 30,
                Margin = new Padding(0, 0, 8, 8),
                BackColor = Surface,
                ForeColor = Primary,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 8.2F),
                Cursor = Cursors.Hand
            };
            button.FlatAppearance.BorderColor = Border;
            button.FlatAppearance.BorderSize = 1;
            button.Click += (s, e) => SubmitQuestion(question);
            return button;
        }

        private Button BuildHeaderButton(string text)
        {
            var button = new Button
            {
                Text = text,
                Size = new Size(32, 32),
                BackColor = Surface,
                ForeColor = Primary,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 13F),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            button.FlatAppearance.BorderSize = 0;
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
            int rowWidth = Math.Max(270, flpMessages.ClientSize.Width - 34);
            int bubbleWidth = Math.Min(Math.Max(220, rowWidth - 42), isUser ? 260 : 280);
            if (Width >= 400)
            {
                bubbleWidth = Math.Min(Math.Max(260, rowWidth - 48), isUser ? 330 : 350);
            }

            var row = new Panel
            {
                Width = rowWidth,
                Height = 1,
                BackColor = Surface,
                Margin = new Padding(0, 0, 0, 10)
            };

            var bubble = new BubblePanel
            {
                Radius = 10,
                BackColor = isUser ? Primary : Surface,
                BorderColor = isUser ? Primary : Border,
                Width = bubbleWidth
            };

            var text = new Label
            {
                Text = message.Text,
                AutoSize = false,
                Font = new Font("Segoe UI", 8.8F),
                ForeColor = isUser ? Color.White : Color.FromArgb(42, 51, 82),
                BackColor = Color.Transparent,
                Location = new Point(12, 10)
            };

            Size measured = TextRenderer.MeasureText(
                text.Text,
                text.Font,
                new Size(bubbleWidth - 24, 0),
                TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix);
            text.Size = new Size(bubbleWidth - 24, Math.Max(22, measured.Height + 4));

            var time = new Label
            {
                Text = message.CreatedAt.ToString("HH:mm"),
                Font = new Font("Segoe UI", 7F),
                ForeColor = isUser ? Color.FromArgb(220, 225, 245) : Muted,
                BackColor = Color.Transparent,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleRight,
                Size = new Size(bubbleWidth - 24, 16),
                Location = new Point(12, text.Bottom + 4)
            };

            bubble.Height = time.Bottom + 10;
            bubble.Controls.Add(text);
            bubble.Controls.Add(time);
            bubble.Left = isUser ? rowWidth - bubbleWidth - 4 : 4;
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
            btnClose.Location = new Point(Width - 46, 24);
            btnExpand.Location = new Point(Width - 84, 24);

            foreach (Control control in flpMessages.Controls)
            {
                control.Width = Math.Max(270, flpMessages.ClientSize.Width - 34);
            }

            int quickWidth = Width >= 400 ? 178 : 150;
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
    }
}
