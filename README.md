# 🏟️ MyStadiumStats

Futbol/Spor maçlarının kapsamlı istatistiklerini takip etmek için tasarlanmış modern bir web uygulaması.

## 📋 İçindekiler
- [Proje Hakkında](#proje-hakkında)
- [Özellikler](#özellikler)
- [Teknik Bilgiler](#teknik-bilgiler)
- [Kurulum Rehberi](#kurulum-rehberi)
- [Kullanım Kılavuzu](#kullanım-kılavuzu)
- [Veritabanı Yapısı](#veritabanı-yapısı)
- [Sistem Gereksinimleri](#sistem-gereksinimleri)

---

## 🎯 Proje Hakkında

**MyStadiumStats**, futbol maçlarının detaylı istatistiklerini, gol scorlarını, oyuncu performansını ve stadyum bilgilerini merkezi bir veritabanında yönetmek için geliştirilmiş bir ASP.NET Core web uygulamasıdır.

### Amaç
- Futbol karşılaşmalarının tüm detaylarını kaydedip organize etme
- Oyuncu ve takım performanslarını analiz etme
- Maç istatistiklerini filtrelenebilir şekilde görüntüleme
- Takım, oyuncu, stadyum ve antrenör bilgilerini merkezi yönetme

### 🤖 Geliştirim Süreci
Bu proje **tamamen Yapay Zeka (AI)** tarafından tasarlanmış, geliştirilmiş ve kodlanmıştır. Mimarisi, tüm özellikleri, veri modelleri, controller'ları, view'ları ve hatta veritabanı migrasyonları AI asistanı tarafından oluşturulmuştur.

---

## ✨ Özellikler

### 📊 Dashboard & İstatistikler
- **Ana Kontrol Paneli**: Tüm istatistiklerin özet görünümü
- **Ev Sahibi/Deplasman Analizi**: Galibiyet, beraberlik, yenilgi ayrımı
- **Gol Scorlarının Ayrı Takibi**: Takımınızın ve tüm oyuncuların gol istatistikleri
- **Maç Türü İstatistikleri**: Resmi, dostluk, turnuva vb. maçlar için ayrı analiz
- **Stadyum İstatistikleri**: Her stadyumda oynanan maçların sonuçlarını takip etme
- **Penaltı Gol Takibi**: Penaltıdan atılan golleri ayrı olarak kaydedip analiz etme

### 🎮 Veri Yönetimi Modülleri

#### 🏆 Takımlar Yönetimi
- Takım oluştur, düzenle, sil
- "Benim Takımım" olarak işaretle
- Takımları takip et
- Ülke bilgisi ekle
- Takıma ait oyuncu ve maçları otomatik görüntüle

#### ⚽ Oyuncu Yönetimi
- Oyuncu profili oluştur (Ad, forma numarası)
- Oyuncu istatistiklerini gör
- Oyuncu performans grafiğini gör
- Oyuncunun oynadığı tüm takımları gör

#### 🎯 Maç Kayıt Sistemi
- Yeni maç ekle (ev sahibi/deplasman takımı seçimi)
- Maç tarihi, stadyum ve skoru gir
- Maç türünü belirle (Resmi, Dostluk, Turnuva vb.)
- Teknik direktörleri atfet
- Maç detaylarını düzenle/sil
- Maç sonuçlarını filtrele

#### ⚾ Gol Kaydı
- Maç başına gol ekle
- Gol atan oyuncuyu belirle
- Golın atıldığı takım ve forma numurasını kaydet
- Penaltı golunu işaretle
- Kendi kalesi golünü işaretle
- Gol dakikasını kaydet

#### 🏟️ Stadyum Yönetimi
- Tüm stadyumları listele
- Stadyumdaki maçları görüntüle
- Stadyum istatistiklerini analiz et
- ℹ️ **Not**: Stadyum yönetim sayfasında doğrudan oluştur/düzenle/sil yoktur; maç eklerken yeni stadyum otomatik oluşturulur

#### 👨‍🏫 Antrenör Yönetimi
- Tüm antrenörleri listele
- Antrenörlerin yer aldığı maçları görüntüle
- Antrenör istatistiklerini takip et
- ℹ️ **Not**: Antrenör yönetim sayfasında doğrudan oluştur/düzenle/sil yoktur; maç eklerken yeni antrenör otomatik oluşturulur

### 🔍 Filtreleme & Arama
- Sonuçlara göre filtrele (Galip, Beraberlik, Yenilgi)
- Oyuncu bazlı gol filtrelemesi
- Penaltı golleri ayrı filtrele
- Maç türüne göre filtrele
- Stadyuma göre filtrele

### 🎨 Arayüz Özellikleri
- **Açık/Koyu Mod**: Gözleri rahatlatacak tema seçeneği
- **Responsive Tasarım**: Mobil, tablet ve masaüstü uyumlu
- **Modern Grafik İnterface**: Chart.js ile görsel istatistikler
- **Dinamik Seçim Menüleri**: Select2 kütüphanesiyle geliştirilmiş arama

---

## 🔧 Teknik Bilgiler

### Framework & Teknolojiler
```
ASP.NET Core 10.0
```

### Mimarı
- **Pattern**: MVC (Model-View-Controller)
- **Veritabanı**: SQLite (Entity Framework Core ORM)
- **Frontend**: HTML5, CSS3, Bootstrap 5
- **Charts/Grafik**: Chart.js
- **UI Enhancements**: Select2, jQuery

### Bağımlılıklar (Packages)
- `Microsoft.EntityFrameworkCore.Design` v10.0.8
- `Microsoft.EntityFrameworkCore.Sqlite` v10.0.8
- `Microsoft.EntityFrameworkCore.Tools` v10.0.8

### Proje Yapısı
```
MyStadiumStats/
├── Controllers/          # İş mantığı ve HTTP yönetimi
│   ├── HomeController.cs     (Dashboard, ana sayfalar)
│   ├── MatchesController.cs  (Maç yönetimi)
│   ├── PlayersController.cs  (Oyuncu yönetimi)
│   ├── TeamsController.cs    (Takım yönetimi)
│   ├── StadiumsController.cs (Stadyum yönetimi)
│   └── CoachesController.cs  (Antrenör yönetimi)
├── Models/               # Veri modelleri
│   ├── Team.cs
│   ├── Player.cs           (Takımdan bağımsız oyuncu modeli)
│   ├── Match.cs
│   ├── Goal.cs             (TeamIdAtMatch ile gol atılan takımı kaydeder)
│   ├── Stadium.cs
│   ├── Coach.cs
│   └── ViewModels/         (Görünüm modelleri)
│       ├── DashboardViewModel.cs
│       └── MatchFormViewModel.cs
├── Views/               # Görünüm şablonları
│   ├── Shared/         (Ortak şablonlar)
│   └── [Controller]/   (Controller başına klasörler)
├── Data/                # Veritabanı bağlamı ve konfigürasyon
│   └── AppDbContext.cs
├── Migrations/          # EF Core veritabanı migrasyonları
│   └── [Migration files]
└── wwwroot/            # Statik dosyalar (CSS, JS, görseller)
```
│   ├── Coach.cs
│   └── ViewModels/
│       ├── DashboardViewModel.cs   (Dashboard veri yapısı)
│       └── MatchFormViewModel.cs   (Maç formu için)
├── Views/                # HTML şablonları
│   ├── Home/             (Dashboard, ana sayfalar)
│   ├── Matches/          (Maç CRUD işlemleri)
│   ├── Players/          (Oyuncu CRUD işlemleri)
│   ├── Teams/            (Takım CRUD işlemleri)
│   ├── Stadiums/         (Stadyum CRUD işlemleri)
│   ├── Coaches/          (Antrenör CRUD işlemleri)
│   └── Shared/           (_Layout, ortak sayfalar)
├── Data/                 # Veritabanı yapılandırması
│   ├── AppDbContext.cs   (EF Core DbContext)
│   └── Migrations/       (Veritabanı versiyonları)
├── wwwroot/              # Statik dosyalar
│   ├── css/              (Stillendirme)
│   ├── js/               (JavaScript)
│   ├── lib/              (Bootstrap, jQuery, Select2)
│   └── favicon.ico       (Site ikonu)
├── Properties/
│   └── launchSettings.json  (Çalıştırma ayarları)
├── Program.cs            # Uygulama başlangıç noktası
├── MyStadiumStats.csproj # Proje dosyası
└── appsettings.json      # Konfigürasyon dosyaları
```

### Veritabanı Bağlantısı
```csharp
SQLite - stadiumstats.db (yerel dosya veritabanı)
Bağlantı Dizesi: Data Source=stadiumstats.db
```

Veritabanı otomatik olarak uygulamanın ilk başlatılmasında oluşturulur ve migration'lar uygulanır.

---

## 📥 Kurulum Rehberi

### Sistem Gereksinimleri
- **.NET 10.0 SDK** veya üzeri ([İndir](https://dotnet.microsoft.com/download))
- **Windows, macOS veya Linux**
- **2GB RAM** (minimum)
- **100MB Disk Alanı**

### Adım 1: Projeyi İndir
```bash
git clone https://github.com/chareless/MyStadiumStats.git
cd MyStadiumStats
```

### Adım 2: Bağımlılıkları Yükle
```bash
dotnet restore
```

### Adım 3: Veritabanını Oluştur ve Migrate Et
```bash
dotnet ef database update
```

*Not: İlk çalıştırmada otomatik olarak yapılır, manuel olarak da yapabilirsiniz.*

### Adım 4: Uygulamayı Çalıştır
```bash
dotnet run
```

### Adım 5: Tarayıcıda Aç
Tarayıcınız otomatik açılacaktır. Eğer açılmazsa şu adresi ziyaret edin:
```
https://localhost:5001
```

---

## 🖱️ Kullanım Kılavuzu

### Başlangıçta Yapılması Gerekenler

#### 1️⃣ Takım Oluştur
1. **Takımlar** menüsüne git
2. **Yeni Takım Ekle** butonuna tıkla
3. Takım adını gir
4. (Opsiyonel) Ülke bilgisini ekle
5. **Kaydet**'e tıkla

#### 2️⃣ "Benim Takımım" Olarak İşaretle
1. Takımlar listesinde takımını bul
2. **Düzenle**'ye tıkla
3. **"Benim Takımım"** kutusunu işaretle
4. **Kaydet**'e tıkla

#### 3️⃣ Stadyum & Antrenör Ekleme
Stadyum ve antrenörleri direkt olarak "Stadyumlar" veya "Teknik Direktörler" sayfasından oluşturmak mümkün değildir, ancak **maç eklerken otomatik olarak oluşturulabilir**:

**Maç ekleme sırasında**:
1. **Maçlar → Yeni Maç Ekle** sayfasına git
2. Stadyum adını yaz (yeni bir stadyum yazarsan otomatik oluşturulur)
3. Teknik direktör adlarını yaz (yeni antrenör yazarsan otomatik oluşturulur)
4. Maç kaydedildiğinde stadyum ve antrenörler de veritabanına eklenir

#### 4️⃣ Oyuncu Ekle
1. **Oyuncular** menüsüne git
2. **Yeni Oyuncu Ekle** butonuna tıkla
3. Oyuncunun adını, takımını ve forma numarasını gir
4. **Kaydet**'e tıkla

### Maç Kaydı Süreci

#### ✅ Yeni Maç Ekle
1. **Maçlar** menüsünden **Yeni Maç Ekle** seçeneğini aç
2. Aşağıdaki bilgileri doldur:
   - **Tarih**: Maçın oynanacağı/oynanış tarihi
   - **Ev Sahibi Takım**: Ev sahibi takımı seç
   - **Deplasman Takımı**: Deplasman takımı seç
   - **Stadyum**: Maçın oynanacağı stadyumu seç
   - **Ev Sahibi Skoru**: Ev sahibi takımın skoru
   - **Deplasman Skoru**: Deplasman takımının skoru
   - **Maç Türü**: Resmi, Dostluk, Turnuva vb. seç
   - **Teknik Direktörler**: (İsteğe bağlı) Antrenörleri ata
3. **Kaydet**'e tıkla

#### ⚽ Maça Gol Ekle
1. Maç listesinden maçı seç
2. **Detaylar** bölümünde **Gol Ekle** butonuna tıkla
3. Gol Kaydı formunu doldur:
   - **Oyuncu**: Gol atanı seç
   - **Dakika**: Gol atılan dakikayı gir
   - **Penaltı Mı?**: Penaltı golüyse işaretle
   - **Asist**: (İsteğe bağlı) Asistanı seç
4. **Kaydet**'e tıkla

### İstatistikleri Görüntüleme

#### 📊 Dashboard'da Genel Bakış Yap
- Ana sayfada önemli istatistikler görüntülenir
- Ev Sahibi/Deplasman galibiyet, beraberlik, yenilgi oranları
- Top skorerler ve takım golcüler
- Maç türüne göre sonuçlar
- Stadyum bazlı istatistikler

#### 🔍 Filtrelenmiş Maçları Görüntüle
Dashboard'daki istatistiklere tıklanarak filtrelenmiş maç listesi gösterilir:
- **Galibiyet sayısına tıkla** → Yalnızca kazanılan maçlar
- **Oyuncu adına tıkla** → O oyuncunun attığı gollerin kaydedildiği maçlar
- **Penaltı sayısına tıkla** → Penaltıdan atılan gollerin olduğu maçlar
- **Maç türüne tıkla** → O türdeki maçlar

### Veri Düzenleme & Silme

#### ✏️ Maç/Oyuncu/Takım Düzenle
1. İlgili menu öğesine git (Maçlar, Oyuncular, vb.)
2. Düzenlemek istediğin öğeyi bul
3. **Düzenle** butonuna tıkla
4. Bilgileri güncelle
5. **Kaydet**'e tıkla

#### 🗑️ Öğe Sil
1. Silinecek öğeyi listede bul
2. **Sil** butonuna tıkla
3. Onay mesajında **Evet**'i seçin
4. Öğe veritabanından silinir

### Tema Ayarları
- Sayfanın sağ üst köşesindeki **"Koyu Mod"** düğmesi ile tema değiştirebilirsin
- Seçiminiz tarayıcı belleğine kaydedilir

---

## 📊 Veritabanı Yapısı

### Tablolar

#### 📌 Teams (Takımlar)
```
Id (PK)
Name (Takım Adı) - Required
Country (Ülke)
IsMyTeam (Benim Takımım Mı?) - Boolean
IsFollowed (Takip Et) - Boolean
```

#### 👤 Players (Oyuncular)
```
Id (PK)
Name (Ad Soyad) - Required
CurrentJerseyNumber (Forma Numarası) - nullable
```
**Not**: Oyuncular takımdan bağımsızdır. Oyuncunun oynadığı takım bilgisi Goals.TeamIdAtMatch üzerinden saklanır, bu sayede aynı oyuncu birden fazla takımda gol atabilir.

#### 🏆 Matches (Maçlar)
```
Id (PK)
Date (Tarih) - Required
HomeTeamId (FK) - Ev Sahibi Takım
AwayTeamId (FK) - Deplasman Takımı
StadiumId (FK) - Stadyum
HomeScore (Ev Sahibi Skoru)
AwayScore (Deplasman Skoru)
MatchType (Maç Türü) - Required
HomeCoachId (FK) - Ev Sahibi Antrenör
AwayCoachId (FK) - Deplasman Antrenörü
```

#### ⚽ Goals (Golleri)
```
Id (PK)
MatchId (FK) - Maç
PlayerId (FK) - Oyuncu
TeamIdAtMatch (FK) - Gol Atılan Takım
Minute (Dakika)
IsOwnGoal (Kendi Kalesi Golü Mü?) - Boolean
IsPenalty (Penaltı Mı?) - Boolean
JerseyNumberAtMatch (Maçtaki Forma Numarası) - nullable
```
**Not**: TeamIdAtMatch, gol atıldığında oyuncunun hangi takımda oynadığını kaydeder. Bu, aynı oyuncunun farklı takımlarda attığı golleri düzgün şekilde takip etmeyi sağlar.

#### 🏟️ Stadiums (Stadyumlar)
```
Id (PK)
Name (Stadyum Adı) - Required
City (Şehir)
Country (Ülke)
```

#### 👨‍🏫 Coaches (Antrenörler)
```
Id (PK)
Name (Ad Soyad) - Required
```

---

## ⚙️ Sistem Gereksinimleri

| Bileşen | Gereksinim |
|---------|-----------|
| **İşletim Sistemi** | Windows 10+, macOS 10.14+, Linux (Ubuntu 18.04+) |
| **.NET SDK** | 10.0 veya üzeri |
| **Veritabanı** | SQLite (dahili) |
| **RAM** | Minimum 2GB |
| **Disk Alanı** | 100MB + veritabanı boyutu |
| **Tarayıcı** | Chrome, Firefox, Safari, Edge (modern sürüm) |
| **İnternet** | Çevrimdışı da çalışır (lokal veritabanı) |

---

## 🔐 Veri Güvenliği & Yedekleme

### Veritabanı Dosyasının Konumu
```
MyStadiumStats/stadiumstats.db
```

### Yedekleme
Veritabanını korumak için aşağıdaki dosyayı düzenli olarak yedekle:
```
stadiumstats.db
```

### Veritabanını Sıfırla (Tüm Veri Silinir)
```bash
dotnet ef database drop -f
dotnet ef database update
```

### Stadyum ve Antrenör Ekleme (Veritabanı Üzerinden)

Stadyum ve antrenörleri maç eklemeden doğrudan oluşturmak istiyorsanız, **DB Browser for SQLite** kullanarak doğrudan veritabanıya veri ekleyebilirsiniz:

**Stadyum Ekleme**:
```sql
INSERT INTO Stadiums (Name, City, Country) 
VALUES ('Adana Stadyumu', 'Adana', 'Türkiye');
```

**Antrenör Ekleme**:
```sql
INSERT INTO Coaches (Name) 
VALUES ('Okan Buruk');
```

Detaylı talimatlar için [SQLite Sorgulama](#🗄️-sqlite-veritabanını-sorgulamak) bölümüne bakın.

> **İpucu**: Çoğu durumda maç eklerken stadyum ve antrenörleri otomatik olarak oluşturmak daha pratiktir!

---

## 🐛 Sorun Giderme

### "Veritabanı bulunamadı" hatası
**Çözüm**:
```bash
dotnet ef database update
```

### Port zaten kullanılıyor
**Çözüm**: `launchSettings.json` dosyasında portu değiştir

### UI öğeleri yüklenmemiş görünüyor
**Çözüm**: 
```bash
dotnet restore
```

---

## 📝 Lisans
Bu proje MIT Lisansı altında lisanslanmıştır. Detaylar için [LICENSE](LICENSE) dosyasını görüntüle.

---

## 👨‍💻 Geliştirme Notları

### ℹ️ Bilinen Sınırlamalar & Özellikler

#### Stadyum ve Antrenör Ekleme
- **Stadyum ve Antrenörleri Ekleme**: Maç eklerken otomatik olarak oluşturulur
  - Maç oluştururken stadyum adını yazın → Yoksa otomatik oluşturulur
  - Teknik direktör adını yazın → Yoksa otomatik oluşturulur
- **Doğrudan Yönetim Sayfası**: Stadyumlar ve Antrenörler sayfalarında Create/Edit/Delete UI henüz yoktur

> Alternatif olarak, [Stadyum ve Antrenör Ekleme (Veritabanı Üzerinden)](#stadyum-ve-antrenör-ekleme-veritabanı-üzerinden) bölümündeki SQL komutlarıyla doğrudan veritabanıya veri ekleyebilirsiniz.

### Gelecek Sürümlerde Planlanan Özellikler
- [ ] Stadyum Yönetimi UI (Create, Edit, Delete)
- [ ] Antrenör Yönetimi UI (Create, Edit, Delete)
- [ ] Kullanıcı hesapları ve kimlik doğrulama
- [ ] Bulut senkronizasyonu
- [ ] Mobil uygulama
- [ ] İleri istatistik analizi (xG, Pass accuracy vb.)
- [ ] Turnuva tablolar sistemi
- [ ] Maç videosu entegrasyonu
- [ ] Bildirim sistemi

---

## 📞 Destek & İletişim

Sorular veya öneriler için lütfen bir Issue açın veya bizimle iletişime geçin.

---

**Versiyon**: 1.0.0  
**Son Güncelleme**: 4 Haziran 2026  
**Durum**: ✅ Aktif Geliştirme  
**GitHub**: https://github.com/chareless/MyStadiumStats

