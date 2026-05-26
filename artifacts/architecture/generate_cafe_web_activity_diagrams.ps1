param(
    [string]$OutputDir = "C:\Users\dung0\OneDrive\Máy tính\ThietKeHeThongT6\BieuDoHoatDongWeb"
)

$ErrorActionPreference = "Stop"

New-Item -ItemType Directory -Force -Path $OutputDir | Out-Null

function Escape-Xml([object]$Value) {
    return [System.Security.SecurityElement]::Escape([string]$Value)
}

function New-Node($Id, $Type, [int]$X, [int]$Y, $Text, [int]$W = 250, [int]$H = 66) {
    [pscustomobject]@{ Id = $Id; Type = $Type; X = $X; Y = $Y; Text = $Text; W = $W; H = $H }
}

function New-Edge($Points, $Label = "", [int]$LX = 0, [int]$LY = 0, [bool]$Arrow = $true) {
    [pscustomobject]@{ Points = $Points; Label = $Label; LX = $LX; LY = $LY; Arrow = $Arrow }
}

function Draw-Text($Cx, $Cy, $Text, $ClassName) {
    $lines = ([string]$Text) -split "\|"
    $lineHeight = 18
    $startY = [int]($Cy - (($lines.Count - 1) * $lineHeight / 2))
    $result = New-Object System.Collections.Generic.List[string]

    for ($i = 0; $i -lt $lines.Count; $i++) {
        $line = Escape-Xml $lines[$i]
        $y = $startY + ($i * $lineHeight)
        $result.Add("  <text x=""$Cx"" y=""$y"" class=""$ClassName"" text-anchor=""middle"" dominant-baseline=""middle"">$line</text>")
    }

    return ($result -join "`n")
}

function Draw-Node($Node) {
    $x = $Node.X
    $y = $Node.Y
    $w = $Node.W
    $h = $Node.H

    if ($Node.Type -eq "start") {
        return "  <circle cx=""$x"" cy=""$y"" r=""16"" fill=""#202020""/>"
    }

    if ($Node.Type -eq "end") {
        return @"
  <circle cx="$x" cy="$y" r="19" fill="#fff" stroke="#3b3b3b" stroke-width="4"/>
  <circle cx="$x" cy="$y" r="10" fill="#202020"/>
"@
    }

    if ($Node.Type -eq "decision") {
        $pts = "$x,$($y - [int]($h / 2)) $($x + [int]($w / 2)),$y $x,$($y + [int]($h / 2)) $($x - [int]($w / 2)),$y"
        return @"
  <polygon points="$pts" fill="#fff7ca" stroke="#ff5151" stroke-width="2"/>
$(Draw-Text $x $y $Node.Text "text")
"@
    }

    $rx = [int]($x - $w / 2)
    $ry = [int]($y - $h / 2)
    return @"
  <rect x="$rx" y="$ry" width="$w" height="$h" rx="18" ry="18" fill="#fff7ca" stroke="#ff5151" stroke-width="2"/>
$(Draw-Text $x $y $Node.Text "text")
"@
}

function Convert-ToOrthogonalPoints($Points) {
    $rawPoints = ([string]$Points) -split " "
    if ($rawPoints.Count -le 1) {
        return $Points
    }

    $parsed = @()
    foreach ($point in $rawPoints) {
        $xy = $point -split ","
        $parsed += [pscustomobject]@{ X = [int]$xy[0]; Y = [int]$xy[1] }
    }

    $result = New-Object System.Collections.Generic.List[string]
    $result.Add("$($parsed[0].X),$($parsed[0].Y)")

    for ($i = 1; $i -lt $parsed.Count; $i++) {
        $prev = $parsed[$i - 1]
        $curr = $parsed[$i]

        if (($prev.X -ne $curr.X) -and ($prev.Y -ne $curr.Y)) {
            $result.Add("$($curr.X),$($prev.Y)")
        }

        $result.Add("$($curr.X),$($curr.Y)")
    }

    return ($result -join " ")
}

function New-ActivityDiagram($Name, [int]$Height, $Nodes, $Edges, $Title = "") {
    $width = 1200
    $laneBottom = $Height - 35
    $parts = New-Object System.Collections.Generic.List[string]

    $parts.Add('<?xml version="1.0" encoding="UTF-8"?>')
    $parts.Add("<svg xmlns=""http://www.w3.org/2000/svg"" width=""$width"" height=""$Height"" viewBox=""0 0 $width $Height"">")
    $parts.Add(@"
  <defs>
    <marker id="arrow" markerWidth="12" markerHeight="12" refX="10" refY="6" orient="auto" markerUnits="strokeWidth">
      <path d="M 0 0 L 12 6 L 0 12 z" fill="#ff1f1f"/>
    </marker>
    <style><![CDATA[
      .lane { font-family: 'Segoe UI', Arial, sans-serif; font-size: 24px; fill: #111; }
      .text { font-family: 'Segoe UI', Arial, sans-serif; font-size: 15px; fill: #111; }
      .label { font-family: 'Segoe UI', Arial, sans-serif; font-size: 13px; font-weight: 600; fill: #111; paint-order: stroke; stroke: #fff; stroke-width: 5px; stroke-linejoin: round; }
      .title { font-family: 'Segoe UI', Arial, sans-serif; font-size: 17px; font-weight: 700; fill: #333; paint-order: stroke; stroke: #fff; stroke-width: 6px; stroke-linejoin: round; }
    ]]></style>
  </defs>
  <rect x="0" y="0" width="$width" height="$Height" fill="#fff"/>
  <line x1="20" y1="30" x2="20" y2="$laneBottom" stroke="#222" stroke-width="3"/>
  <line x1="300" y1="30" x2="300" y2="$laneBottom" stroke="#222" stroke-width="3"/>
  <line x1="610" y1="30" x2="610" y2="$laneBottom" stroke="#222" stroke-width="3"/>
  <line x1="1180" y1="30" x2="1180" y2="$laneBottom" stroke="#222" stroke-width="3"/>
  <text x="160" y="58" class="lane" text-anchor="middle">Người dùng</text>
  <text x="455" y="58" class="lane" text-anchor="middle">Giao diện web</text>
  <text x="895" y="58" class="lane" text-anchor="middle">Hệ thống / CSDL</text>
"@)

    if ($Title) {
        $parts.Add("  <text x=""600"" y=""90"" class=""title"" text-anchor=""middle"">$(Escape-Xml $Title)</text>")
    }

    foreach ($node in $Nodes) {
        $parts.Add((Draw-Node $node))
    }

    foreach ($edge in $Edges) {
        $marker = if ($edge.Arrow) { ' marker-end="url(#arrow)"' } else { "" }
        $points = Convert-ToOrthogonalPoints $edge.Points
        $parts.Add("  <polyline points=""$points"" fill=""none"" stroke=""#ff1f1f"" stroke-width=""2.5"" stroke-linejoin=""round"" stroke-linecap=""round""$marker/>")
        if ($edge.Label) {
            $parts.Add("  <text x=""$($edge.LX)"" y=""$($edge.LY)"" class=""label"" text-anchor=""middle"">$(Escape-Xml $edge.Label)</text>")
        }
    }

    $parts.Add("</svg>")

    $svgPath = Join-Path $OutputDir ($Name + ".svg")
    Set-Content -LiteralPath $svgPath -Value ($parts -join "`n") -Encoding UTF8
    return $svgPath
}

function Render-Png($SvgPath) {
    $browserCandidates = @(
        "C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe",
        "C:\Program Files\Microsoft\Edge\Application\msedge.exe",
        "C:\Program Files\Google\Chrome\Application\chrome.exe",
        "C:\Program Files (x86)\Google\Chrome\Application\chrome.exe"
    )
    $browser = $browserCandidates | Where-Object { Test-Path -LiteralPath $_ } | Select-Object -First 1
    if (-not $browser) {
        throw "Không tìm thấy Edge/Chrome để render PNG."
    }

    [xml]$svg = Get-Content -Raw -Encoding UTF8 -LiteralPath $SvgPath
    $w = [int]$svg.DocumentElement.width
    $h = [int]$svg.DocumentElement.height
    $pngPath = [System.IO.Path]::ChangeExtension($SvgPath, ".png")
    $uri = [System.Uri]::new((Get-Item -LiteralPath $SvgPath).FullName).AbsoluteUri
    $tmp = Join-Path ([System.IO.Path]::GetTempPath()) ("CafeActivityRender_" + [System.Guid]::NewGuid().ToString("N"))

    New-Item -ItemType Directory -Force -Path $tmp | Out-Null
    try {
        $args = @(
            "--headless=new",
            "--disable-gpu",
            "--hide-scrollbars",
            "--no-first-run",
            "--disable-extensions",
            "--force-device-scale-factor=1",
            "--user-data-dir=$tmp",
            "--window-size=$w,$h",
            "--screenshot=$pngPath",
            $uri
        )
        $argLine = ($args | ForEach-Object { '"' + ($_ -replace '"', '\"') + '"' }) -join " "
        $p = Start-Process -FilePath $browser -ArgumentList $argLine -RedirectStandardOutput (Join-Path $tmp "stdout.txt") -RedirectStandardError (Join-Path $tmp "stderr.txt") -WindowStyle Hidden -Wait -PassThru
        if ($p.ExitCode -ne 0) {
            throw "Render PNG failed: $($p.ExitCode) for $SvgPath"
        }
    }
    finally {
        Remove-Item -LiteralPath $tmp -Recurse -Force -ErrorAction SilentlyContinue
    }
}

$U = 160
$UI = 455
$S = 895
$diagrams = @()

$diagrams += [pscustomobject]@{
    Name = "HoatDongWeb_DangNhap"
    Height = 1260
    Title = "Đăng nhập và phân quyền"
    Nodes = @(
        New-Node "st" "start" $U 110 "" 0 0
        New-Node "u1" "action" $U 180 "Nhập tên đăng nhập|và mật khẩu" 230 64
        New-Node "ui1" "action" $UI 180 "Kiểm tra dữ liệu|đăng nhập" 250 64
        New-Node "d1" "decision" $UI 305 "Dữ liệu|hợp lệ?" 180 86
        New-Node "err1" "action" $UI 430 "Hiển thị lỗi|và yêu cầu nhập lại" 260 64
        New-Node "endInput" "end" $UI 515 "" 0 0
        New-Node "sys1" "action" $S 430 "Mã hóa mật khẩu MD5|và truy vấn TaiKhoan" 340 64
        New-Node "d2" "decision" $S 555 "Tài khoản|đúng?" 190 86
        New-Node "err2" "action" $UI 680 "Thông báo đăng nhập|thất bại" 270 64
        New-Node "endFail" "end" $UI 765 "" 0 0
        New-Node "sys2" "action" $S 680 "Lấy thông tin nhân viên|và tạo Session" 340 64
        New-Node "d3" "decision" $S 805 "Vai trò|Admin?" 180 86
        New-Node "admin" "action" $UI 930 "Mở màn hình quản trị|frmMain" 270 64
        New-Node "staff" "action" $UI 1060 "Mở màn hình nhân viên|frmMainNhanVien" 280 64
        New-Node "end" "end" $UI 1185 "" 0 0
    )
    Edges = @(
        New-Edge "$U,126 $U,148"
        New-Edge "$($U + 115),180 $($UI - 125),180"
        New-Edge "$UI,212 $UI,262"
        New-Edge "$($UI + 90),305 $S,305 $S,398" "Có" 570 292
        New-Edge "$UI,348 $UI,398" "Không" 410 374
        New-Edge "$UI,462 $UI,496"
        New-Edge "$S,462 $S,512"
        New-Edge "$($S - 95),555 $($UI + 135),555 $UI,648" "Không" 690 535
        New-Edge "$S,598 $S,648" "Có" 940 625
        New-Edge "$UI,712 $UI,746"
        New-Edge "$S,712 $S,762"
        New-Edge "$($S - 90),805 $($UI + 135),805 $UI,898" "Có" 700 786
        New-Edge "$S,848 $S,1060 $($UI + 140),1060" "Không" 945 920
        New-Edge "$UI,962 340,962 340,1185 436,1185"
        New-Edge "$UI,1092 $UI,1166"
    )
}

$diagrams += [pscustomobject]@{
    Name = "HoatDongWeb_BanHang_GoiMon_ThanhToan"
    Height = 1680
    Title = "Bán hàng, gọi món và thanh toán"
    Nodes = @(
        New-Node "st" "start" $U 110 "" 0 0
        New-Node "u1" "action" $U 180 "Chọn bàn trên|sơ đồ bán hàng" 230 64
        New-Node "ui1" "action" $UI 180 "Mở màn hình gọi món|load khách, menu, hóa đơn" 285 70
        New-Node "sys1" "action" $S 180 "Tìm hóa đơn chưa thanh toán|theo bàn" 340 64
        New-Node "d1" "decision" $S 315 "Bàn đã|mở?" 170 86
        New-Node "open" "action" $UI 430 "Tạo hóa đơn mới|cập nhật bàn Có người" 295 70
        New-Node "show" "action" $UI 555 "Hiển thị menu|và giỏ hàng" 260 64
        New-Node "u2" "action" $U 680 "Chọn khách hàng, món|và số lượng" 245 70
        New-Node "sys2" "action" $S 680 "Kiểm tra tồn kho|theo công thức món" 340 64
        New-Node "d2" "decision" $S 815 "Đủ nguyên|liệu?" 180 86
        New-Node "err" "action" $UI 930 "Thông báo không đủ|nguyên liệu" 260 64
        New-Node "add" "action" $S 930 "Thêm / cập nhật|ChiTietHoaDon" 320 64
        New-Node "bill" "action" $UI 1050 "Tính tổng tiền|và giảm giá điểm" 280 64
        New-Node "pay" "action" $U 1170 "Bấm thanh toán|và xác nhận" 230 64
        New-Node "sys3" "action" $S 1170 "Cập nhật HoaDon, bàn trống|và trừ tồn kho" 355 70
        New-Node "d3" "decision" $S 1310 "Có khách|hàng?" 180 86
        New-Node "point" "action" $S 1430 "Cộng điểm tích lũy|cho khách hàng" 330 64
        New-Node "ok" "action" $UI 1545 "Thông báo thành công|hỏi in hóa đơn" 280 64
        New-Node "end" "end" $UI 1630 "" 0 0
    )
    Edges = @(
        New-Edge "$U,126 $U,148"
        New-Edge "$($U + 115),180 $($UI - 142),180"
        New-Edge "$($UI + 142),180 $($S - 170),180"
        New-Edge "$S,212 $S,272"
        New-Edge "$($S - 85),315 $UI,315 $UI,395" "Không" 650 295
        New-Edge "$S,358 $S,510 $($UI + 130),510" "Có" 940 455
        New-Edge "$UI,465 $UI,523"
        New-Edge "$UI,587 $UI,630 $($U + 122),630 $U,645"
        New-Edge "$($U + 122),680 $($S - 170),680"
        New-Edge "$S,712 $S,772"
        New-Edge "$($S - 90),815 $UI,815 $UI,898" "Không" 655 795
        New-Edge "$S,858 $S,898" "Có" 940 885
        New-Edge "$S,962 $S,1018 $($UI + 140),1018"
        New-Edge "$UI,1082 $UI,1125 $($U + 115),1125 $U,1138"
        New-Edge "$($U + 115),1170 $($S - 178),1170"
        New-Edge "$S,1205 $S,1267"
        New-Edge "$S,1353 $S,1398" "Có" 940 1376
        New-Edge "$($S - 90),1310 $UI,1310 $UI,1513" "Không" 650 1290
        New-Edge "$S,1462 $S,1505 $($UI + 140),1505"
        New-Edge "$UI,1577 $UI,1611"
    )
}

$diagrams += [pscustomobject]@{
    Name = "HoatDongWeb_QuanLyBan"
    Height = 1380
    Title = "Quản lý bàn"
    Nodes = @(
        New-Node "st" "start" $U 110 "" 0 0
        New-Node "u1" "action" $U 180 "Admin mở chức năng|Quản lý bàn" 230 64
        New-Node "sys1" "action" $S 180 "Đọc danh sách Ban|từ CSDL" 320 64
        New-Node "ui1" "action" $UI 305 "Hiển thị bảng bàn|kèm trạng thái" 270 64
        New-Node "u2" "action" $U 430 "Tìm kiếm, chọn dòng|hoặc nhập thông tin" 250 70
        New-Node "d1" "decision" $UI 555 "Thao tác|xóa?" 170 86
        New-Node "sys2" "action" $S 555 "Kiểm tra lịch sử hóa đơn|liên quan bàn" 350 64
        New-Node "d2" "decision" $S 680 "Có ràng|buộc?" 170 86
        New-Node "err" "action" $UI 805 "Thông báo không thể xóa|vì có lịch sử hóa đơn" 300 70
        New-Node "sys3" "action" $S 805 "Thêm / sửa / ẩn / khôi phục|hoặc xóa bàn" 350 70
        New-Node "ui2" "action" $UI 940 "Làm mới danh sách|và xóa form nhập" 280 64
        New-Node "end" "end" $UI 1050 "" 0 0
    )
    Edges = @(
        New-Edge "$U,126 $U,148"
        New-Edge "$($U + 115),180 $($S - 160),180"
        New-Edge "$S,212 $S,260 $($UI + 135),260 $UI,273"
        New-Edge "$UI,337 $UI,385 $($U + 125),385 $U,395"
        New-Edge "$($U + 125),430 $UI,430 $UI,512"
        New-Edge "$($UI + 85),555 $($S - 175),555" "Có" 630 535
        New-Edge "$UI,598 $UI,760 $($S - 175),760 $S,770" "Không" 545 660
        New-Edge "$S,587 $S,637"
        New-Edge "$($S - 85),680 $UI,680 $UI,770" "Có" 690 660
        New-Edge "$S,723 $S,770" "Không" 940 748
        New-Edge "$UI,840 $UI,908"
        New-Edge "$S,840 $S,895 $($UI + 140),895 $UI,908"
        New-Edge "$UI,972 $UI,1031"
    )
}

$diagrams += [pscustomobject]@{
    Name = "HoatDongWeb_DatBan"
    Height = 1400
    Title = "Đặt bàn trước"
    Nodes = @(
        New-Node "st" "start" $U 110 "" 0 0
        New-Node "u1" "action" $U 180 "Mở chức năng|Đặt bàn" 220 64
        New-Node "sys1" "action" $S 180 "Load bàn, khách hàng|và lịch sử đặt bàn" 340 70
        New-Node "ui1" "action" $UI 305 "Hiển thị form đặt bàn|và danh sách phiếu" 290 70
        New-Node "u2" "action" $U 430 "Nhập bàn, khách, ngày giờ|số người, ghi chú" 260 72
        New-Node "sys2" "action" $S 430 "Kiểm tra dữ liệu|và bàn được chọn" 330 64
        New-Node "d1" "decision" $S 555 "Thông tin|hợp lệ?" 180 86
        New-Node "err" "action" $UI 680 "Hiển thị lỗi|yêu cầu nhập lại" 260 64
        New-Node "save" "action" $S 680 "Tạo DatBanTruoc|trạng thái Chờ xác nhận" 350 70
        New-Node "u3" "action" $U 805 "Chọn phiếu đặt bàn|để xác nhận / hủy / xóa" 265 72
        New-Node "sys3" "action" $S 805 "Cập nhật trạng thái|hoặc xóa phiếu đặt" 350 70
        New-Node "ui2" "action" $UI 940 "Làm mới danh sách|và thông báo kết quả" 290 64
        New-Node "end" "end" $UI 1060 "" 0 0
    )
    Edges = @(
        New-Edge "$U,126 $U,148"
        New-Edge "$($U + 110),180 $($S - 170),180"
        New-Edge "$S,215 $S,260 $($UI + 145),260 $UI,270"
        New-Edge "$UI,340 $UI,385 $($U + 130),385 $U,394"
        New-Edge "$($U + 130),430 $($S - 165),430"
        New-Edge "$S,462 $S,512"
        New-Edge "$($S - 90),555 $UI,555 $UI,648" "Không" 690 535
        New-Edge "$S,598 $S,645" "Có" 940 625
        New-Edge "$S,715 $S,755 $($U + 132),755 $U,769"
        New-Edge "$($U + 132),805 $($S - 175),805"
        New-Edge "$S,840 $S,895 $($UI + 145),895 $UI,908"
        New-Edge "$UI,972 $UI,1041"
    )
}

$diagrams += [pscustomobject]@{
    Name = "HoatDongWeb_Chuyen_GopBan"
    Height = 1770
    Title = "Chuyển bàn và gộp bàn"
    Nodes = @(
        New-Node "st" "start" $U 110 "" 0 0
        New-Node "u1" "action" $U 180 "Chọn bàn đang có khách|trên sơ đồ" 245 68
        New-Node "ui1" "action" $UI 180 "Mở màn hình gọi món|của bàn hiện tại" 285 68
        New-Node "sys1" "action" $S 180 "Lấy hóa đơn chưa thanh toán|của bàn" 350 64
        New-Node "d1" "decision" $S 315 "Có hóa|đơn?" 170 86
        New-Node "err" "action" $UI 430 "Thông báo bàn|chưa được mở" 260 64
        New-Node "endErr" "end" $UI 525 "" 0 0
        New-Node "d2" "decision" $UI 620 "Chức năng|được chọn?" 185 86
        New-Node "cb1" "action" $S 760 "Load danh sách bàn trống|để chuyển bàn" 350 64
        New-Node "u2" "action" $UI 900 "Chọn bàn mới|và lý do chuyển" 270 64
        New-Node "sys2" "action" $S 1030 "Cập nhật HoaDon.IdBan,|trạng thái bàn, log chuyển" 365 72
        New-Node "gb1" "action" $S 1160 "Load danh sách bàn có người|để gộp hóa đơn" 350 64
        New-Node "u3" "action" $UI 1300 "Chọn bàn phụ|cần gộp" 260 64
        New-Node "sys3" "action" $S 1430 "Gộp ChiTietHoaDon, xóa hóa đơn phụ,|đặt bàn phụ Trống, ghi log" 390 72
        New-Node "ui2" "action" $UI 1580 "Load lại hóa đơn|và sơ đồ bàn" 280 64
        New-Node "end" "end" $UI 1680 "" 0 0
    )
    Edges = @(
        New-Edge "$U,126 $U,146"
        New-Edge "$($U + 122),180 $($UI - 142),180"
        New-Edge "$($UI + 142),180 $($S - 175),180"
        New-Edge "$S,212 $S,272"
        New-Edge "$($S - 85),315 $UI,315 $UI,398" "Không" 675 294
        New-Edge "$S,358 $S,560 $UI,560 $UI,577" "Có" 940 460
        New-Edge "$UI,462 $UI,506"
        New-Edge "$($UI + 92),620 $S,620 $S,728" "Chuyển bàn" 650 600
        New-Edge "$UI,663 620,663 620,1125 $S,1125 $S,1128" "Gộp bàn" 640 705
        New-Edge "$S,792 $S,850 $UI,850 $UI,868"
        New-Edge "$($UI + 135),900 $S,900 $S,994"
        New-Edge "$S,1066 650,1066 650,1545 $($UI + 140),1545 $UI,1548"
        New-Edge "$S,1192 $S,1245 $UI,1245 $UI,1268"
        New-Edge "$($UI + 130),1300 $S,1300 $S,1394"
        New-Edge "$S,1466 $S,1525 $($UI + 140),1525 $UI,1548"
        New-Edge "$UI,1612 $UI,1661"
    )
}

$diagrams += [pscustomobject]@{
    Name = "HoatDongWeb_DoUong_DanhMuc"
    Height = 1370
    Title = "Quản lý đồ uống và danh mục"
    Nodes = @(
        New-Node "st" "start" $U 110 "" 0 0
        New-Node "u1" "action" $U 180 "Admin mở quản lý|đồ uống / danh mục" 250 68
        New-Node "sys1" "action" $S 180 "Load LoaiDoUong|và DoUong" 320 64
        New-Node "ui1" "action" $UI 305 "Hiển thị danh sách|và form nhập" 280 64
        New-Node "u2" "action" $U 430 "Nhập tên, giá, loại|và hình ảnh" 245 68
        New-Node "d1" "decision" $UI 555 "Thao tác|xóa?" 170 86
        New-Node "sys2" "action" $S 555 "Kiểm tra bản ghi|và ràng buộc dữ liệu" 350 64
        New-Node "d2" "decision" $S 680 "Có thể|xóa?" 170 86
        New-Node "err" "action" $UI 805 "Thông báo lỗi|không thể xóa" 260 64
        New-Node "sys3" "action" $S 805 "Thêm / sửa / xóa|DoUong hoặc LoaiDoUong" 355 70
        New-Node "ui2" "action" $UI 940 "Làm mới danh sách|và thông báo kết quả" 290 64
        New-Node "end" "end" $UI 1050 "" 0 0
    )
    Edges = @(
        New-Edge "$U,126 $U,146"
        New-Edge "$($U + 125),180 $($S - 160),180"
        New-Edge "$S,212 $S,260 $($UI + 140),260 $UI,273"
        New-Edge "$UI,337 $UI,385 $($U + 122),385 $U,396"
        New-Edge "$($U + 122),430 $UI,430 $UI,512"
        New-Edge "$($UI + 85),555 $($S - 175),555" "Có" 630 535
        New-Edge "$UI,598 $UI,760 $($S - 178),760 $S,770" "Không" 545 660
        New-Edge "$S,587 $S,637"
        New-Edge "$($S - 85),680 $UI,680 $UI,773" "Không" 690 660
        New-Edge "$S,723 $S,770" "Có" 940 748
        New-Edge "$UI,837 $UI,908"
        New-Edge "$S,840 $S,895 $($UI + 145),895 $UI,908"
        New-Edge "$UI,972 $UI,1031"
    )
}

$diagrams += [pscustomobject]@{
    Name = "HoatDongWeb_KhachHang_TichDiem"
    Height = 1580
    Title = "Khách hàng và tích điểm"
    Nodes = @(
        New-Node "st" "start" $U 110 "" 0 0
        New-Node "u1" "action" $U 180 "Mở chức năng|Khách hàng / tích điểm" 255 68
        New-Node "sys1" "action" $S 180 "Load danh sách KhachHang|và điểm tích lũy" 350 68
        New-Node "ui1" "action" $UI 305 "Hiển thị danh sách|khách hàng" 270 64
        New-Node "u2" "action" $U 430 "Tìm theo SĐT / tên|hoặc tạo mới" 245 68
        New-Node "d1" "decision" $S 555 "Khách hàng|tồn tại?" 190 86
        New-Node "new" "action" $UI 705 "Nhập thông tin|khách hàng mới" 270 68
        New-Node "old" "action" $S 705 "Hiển thị hồ sơ|và điểm hiện có" 300 68
        New-Node "sys2" "action" $S 855 "Kiểm tra dữ liệu|và điểm hiện có" 340 64
        New-Node "d2" "decision" $S 985 "Thao tác|điểm?" 180 86
        New-Node "add" "action" $UI 1130 "Cộng điểm từ|hóa đơn thanh toán" 280 68
        New-Node "use" "action" $S 1130 "Đổi điểm|giảm giá" 260 68
        New-Node "d3" "decision" $S 1260 "Đủ điểm|để đổi?" 180 86
        New-Node "err" "action" $UI 1390 "Thông báo|không đủ điểm" 250 64
        New-Node "save" "action" $S 1390 "Lưu thay đổi điểm|và cập nhật khách hàng" 350 68
        New-Node "end" "end" $S 1500 "" 0 0
    )
    Edges = @(
        New-Edge "$U,126 $U,146"
        New-Edge "$($U + 127),180 $($S - 175),180"
        New-Edge "$S,214 $S,260 $($UI + 135),260 $UI,273"
        New-Edge "$UI,337 $UI,385 $($U + 122),385 $U,396"
        New-Edge "$($U + 122),430 $S,430 $S,512"
        New-Edge "$($S - 95),555 $UI,555 $UI,671" "Không" 690 535
        New-Edge "$S,598 $S,671" "Có" 940 625
        New-Edge "$UI,739 $UI,800 $S,800 $S,823"
        New-Edge "$S,739 $S,823"
        New-Edge "$S,887 $S,942"
        New-Edge "$($S - 90),985 $UI,985 $UI,1096" "Cộng" 690 965
        New-Edge "$S,1028 $S,1096" "Đổi" 940 1055
        New-Edge "$UI,1164 $UI,1215 $S,1215 $S,1217"
        New-Edge "$S,1164 $S,1217"
        New-Edge "$($S - 90),1260 $UI,1260 $UI,1358" "Không" 690 1240
        New-Edge "$S,1303 $S,1356" "Có" 940 1330
        New-Edge "$UI,1422 $UI,1460 $S,1460 $S,1481"
        New-Edge "$S,1424 $S,1481"
    )
}

$diagrams += [pscustomobject]@{
    Name = "HoatDongWeb_NhapKho_NguyenLieu"
    Height = 1500
    Title = "Nhập kho nguyên liệu"
    Nodes = @(
        New-Node "st" "start" $U 110 "" 0 0
        New-Node "u1" "action" $U 180 "Mở chức năng|Nhập kho" 220 64
        New-Node "sys1" "action" $S 180 "Load nhà cung cấp, nguyên liệu|và lịch sử nhập kho" 370 70
        New-Node "ui1" "action" $UI 305 "Hiển thị phiếu nhập|và danh sách lịch sử" 300 70
        New-Node "u2" "action" $U 430 "Chọn / thêm nhà cung cấp|nhập ghi chú" 260 70
        New-Node "u3" "action" $U 555 "Chọn nguyên liệu, số lượng,|đơn giá rồi thêm vào phiếu" 270 72
        New-Node "ui2" "action" $UI 680 "Cập nhật giỏ nhập|và tổng tiền" 280 64
        New-Node "d1" "decision" $UI 805 "Giỏ nhập|rỗng?" 170 86
        New-Node "err" "action" $UI 930 "Thông báo cần thêm|nguyên liệu" 260 64
        New-Node "sys2" "action" $S 930 "Tạo PhieuNhapKho|và số phiếu PN-yyyyMMdd" 360 70
        New-Node "sys3" "action" $S 1065 "Lưu ChiTietPhieuNhap|và cộng tồn NguyenLieu" 370 70
        New-Node "sys4" "action" $S 1200 "Cập nhật tổng tiền|và commit giao dịch" 350 64
        New-Node "ui3" "action" $UI 1320 "Thông báo thành công|làm mới lịch sử" 290 64
        New-Node "end" "end" $UI 1420 "" 0 0
    )
    Edges = @(
        New-Edge "$U,126 $U,148"
        New-Edge "$($U + 110),180 $($S - 185),180"
        New-Edge "$S,215 $S,260 $($UI + 150),260 $UI,270"
        New-Edge "$UI,340 $UI,385 $($U + 130),385 $U,395"
        New-Edge "$U,465 $U,519"
        New-Edge "$($U + 135),555 $UI,555 $UI,648"
        New-Edge "$UI,712 $UI,762"
        New-Edge "$UI,848 $UI,898" "Có" 505 875
        New-Edge "$($UI + 85),805 $($S - 180),805 $S,895" "Không" 650 785
        New-Edge "$S,965 $S,1030"
        New-Edge "$S,1100 $S,1168"
        New-Edge "$S,1232 $S,1275 $($UI + 145),1275 $UI,1288"
        New-Edge "$UI,1352 $UI,1401"
    )
}

$diagrams += [pscustomobject]@{
    Name = "HoatDongWeb_NhanSu_ChamCong_Luong"
    Height = 1600
    Title = "Chấm công và tính lương"
    Nodes = @(
        New-Node "st" "start" $U 110 "" 0 0
        New-Node "u1" "action" $U 180 "Admin mở chức năng|Chấm công / Lương" 255 68
        New-Node "sys1" "action" $S 180 "Load nhân viên|và ca làm việc" 330 64
        New-Node "ui1" "action" $UI 305 "Hiển thị danh sách|chấm công" 270 64
        New-Node "u2" "action" $U 430 "Nhập nhân viên, ca làm,|giờ vào và giờ ra" 260 70
        New-Node "sys2" "action" $S 430 "So sánh giờ vào/ra|với ca làm" 340 64
        New-Node "d1" "decision" $S 555 "Trạng thái|làm việc?" 190 86
        New-Node "cc" "action" $S 690 "Lưu ChamCong|Đúng giờ / Đi muộn / Về sớm" 370 72
        New-Node "u3" "action" $U 830 "Chọn tháng/năm|và bấm tính lương" 250 68
        New-Node "sys3" "action" $S 830 "Xóa lương cũ, lấy chấm công|theo nhân viên" 380 70
        New-Node "sys4" "action" $S 965 "Tính ngày công, giờ làm,|thưởng, phạt, tổng lương" 380 72
        New-Node "sys5" "action" $S 1100 "Lưu LuongNhanVien|trạng thái chưa thanh toán" 365 70
        New-Node "ui2" "action" $UI 1230 "Hiển thị bảng lương|theo tháng" 280 64
        New-Node "u4" "action" $U 1350 "Chọn dòng lương|và thanh toán" 235 64
        New-Node "sys6" "action" $S 1350 "Cập nhật DaThanhToan|và ngày thanh toán" 360 64
        New-Node "end" "end" $UI 1480 "" 0 0
    )
    Edges = @(
        New-Edge "$U,126 $U,146"
        New-Edge "$($U + 127),180 $($S - 165),180"
        New-Edge "$S,212 $S,260 $($UI + 135),260 $UI,273"
        New-Edge "$UI,337 $UI,385 $($U + 130),385 $U,395"
        New-Edge "$($U + 130),430 $($S - 170),430"
        New-Edge "$S,462 $S,512"
        New-Edge "$S,598 $S,654" "Đúng / muộn / sớm" 980 624
        New-Edge "$S,726 $S,780 $($U + 125),780 $U,796"
        New-Edge "$($U + 125),830 $($S - 190),830"
        New-Edge "$S,865 $S,929"
        New-Edge "$S,1001 $S,1065"
        New-Edge "$S,1135 $S,1185 $($UI + 140),1185 $UI,1198"
        New-Edge "$UI,1262 $UI,1305 $($U + 117),1305 $U,1318"
        New-Edge "$($U + 117),1350 $($S - 180),1350"
        New-Edge "$S,1382 $S,1430 $($UI + 19),1430 $UI,1461"
    )
}

$diagrams += [pscustomobject]@{
    Name = "HoatDongWeb_BaoCao_ThongKe"
    Height = 1400
    Title = "Báo cáo doanh thu và thống kê"
    Nodes = @(
        New-Node "st" "start" $U 110 "" 0 0
        New-Node "u1" "action" $U 180 "Admin mở chức năng|Thống kê / Báo cáo" 250 68
        New-Node "ui1" "action" $UI 180 "Hiển thị bộ lọc|từ ngày - đến ngày" 285 64
        New-Node "u2" "action" $U 305 "Chọn khoảng thời gian|và loại báo cáo" 255 68
        New-Node "sys1" "action" $S 305 "Truy vấn hóa đơn|đã thanh toán" 340 64
        New-Node "sys2" "action" $S 430 "Join ChiTietHoaDon, DoUong,|KhachHang và nhóm dữ liệu" 380 72
        New-Node "sys3" "action" $S 565 "Tính doanh thu, số lượng bán,|top món và lợi nhuận 30%" 380 72
        New-Node "ui2" "action" $UI 705 "Hiển thị card thống kê,|bảng dữ liệu và biểu đồ" 300 72
        New-Node "d1" "decision" $UI 845 "Xuất|Excel?" 160 82
        New-Node "sys4" "action" $S 970 "Tạo file báo cáo .xls|theo thời gian hiện tại" 360 70
        New-Node "ui3" "action" $UI 1100 "Thông báo kết quả|và mở chi tiết nếu cần" 290 64
        New-Node "end" "end" $UI 1220 "" 0 0
    )
    Edges = @(
        New-Edge "$U,126 $U,146"
        New-Edge "$($U + 125),180 $($UI - 142),180"
        New-Edge "$UI,212 $UI,260 $($U + 127),260 $U,271"
        New-Edge "$($U + 127),305 $($S - 170),305"
        New-Edge "$S,337 $S,394"
        New-Edge "$S,466 $S,529"
        New-Edge "$S,601 $S,660 $($UI + 150),660 $UI,669"
        New-Edge "$UI,741 $UI,804"
        New-Edge "$($UI + 80),845 $($S - 180),845 $S,935" "Có" 650 825
        New-Edge "$UI,886 $UI,1068" "Không" 505 920
        New-Edge "$S,1005 $S,1045 $($UI + 145),1045 $UI,1068"
        New-Edge "$UI,1132 $UI,1201"
    )
}

$svgPaths = New-Object System.Collections.Generic.List[string]
foreach ($diagram in $diagrams) {
    $svgPaths.Add((New-ActivityDiagram $diagram.Name $diagram.Height $diagram.Nodes $diagram.Edges $diagram.Title))
}

foreach ($svgPath in $svgPaths) {
    Render-Png $svgPath
}

Get-ChildItem -LiteralPath $OutputDir -Filter "HoatDongWeb_*.*" |
    Sort-Object Name |
    Select-Object Name, Length, LastWriteTime
