# SmartPOS Phone Scanner

Flutter companion app for SmartPOS WinForms. The app scans the QR URL shown by the WinForms phone bridge, then scans product barcodes and posts them to the existing `/api/code` endpoint.

## Flow

1. In SmartPOS WinForms, open the phone scanning dialog once to show the bridge QR.
2. On the phone, open this app and scan the QR shown by WinForms.
3. After the bridge is connected, the WinForms dialog can be closed; the bridge server keeps running until SmartPOS exits.
4. Scan product barcodes on the phone. The app asks WinForms for product details, keeps a local cart, and shows the transfer QR at checkout.

## Build

Install Flutter SDK, then run:

```powershell
cd SmartPOS.PhoneScanner
flutter pub get
flutter run
```

For Android release APK:

```powershell
flutter build apk --release
```

The Android app allows cleartext HTTP because the WinForms bridge uses a local LAN URL such as `http://192.168.x.x:port/`.

The WinForms bridge prefers port `5055`. If that port is occupied, the QR contains the fallback port selected at runtime.
