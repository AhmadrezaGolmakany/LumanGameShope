# 🎮 Luman Game Shop

بک‌اند یک فروشگاه اینترنتی محصولات گیمینگ، پیاده‌سازی‌شده با **ASP.NET Core Web API** بر پایه معماری سه‌لایه (N-Layer).

این پروژه به‌عنوان یک نمونه‌کار (Portfolio Project) توسعه داده شده و امکاناتی مثل احراز هویت با JWT، مدیریت محصولات و دسته‌بندی‌ها، سبد خرید و سفارش، کد تخفیف، لیست علاقه‌مندی‌ها و پنل مدیریت را پوشش می‌دهد.

---

## 📐 معماری پروژه

پروژه بر پایه معماری **سه‌لایه** طراحی شده تا مسئولیت‌ها به‌درستی از هم جدا باشند:

```
LumanGameShope/
├── Luman.Api/              # لایه ارائه (Presentation) — کنترلرها، تنظیمات JWT، Swagger
├── Luman.Busines/          # لایه بیزینس (Business/Service) — سرویس‌ها، DTOها، AutoMapper، ابزارها
└── Luman.DataLayer/        # لایه داده (Data Access) — DbContext، Entity Modelها، Migrationها
```

- **Luman.DataLayer**: شامل مدل‌های Entity Framework Core (`User`, `Product`, `Category`, `Order`, `Discount`, `Role`, `Permition` و ...) و مدیریت Migrationهای دیتابیس.
- **Luman.Busines**: منطق اصلی برنامه شامل سرویس‌ها (`UserServices`, `ProductService`, `OrderServices`, `PermissionService`)، DTOهای ورودی/خروجی، پروفایل‌های AutoMapper و ابزارهای کمکی مثل هش کردن پسورد.
- **Luman.Api**: نقطه ورود برنامه، شامل کنترلرهای REST API، تنظیمات احراز هویت JWT، Swagger/OpenAPI و API Versioning.

---

## 🚀 تکنولوژی‌ها و پکیج‌ها

| بخش | تکنولوژی |
|---|---|
| فریم‌ورک | ASP.NET Core Web API |
| ORM | Entity Framework Core (SQL Server) |
| احراز هویت | JWT Bearer Authentication |
| نگاشت اشیاء | AutoMapper |
| مستندسازی API | Swagger / Swashbuckle |
| نسخه‌بندی API | Microsoft.AspNetCore.Mvc.Versioning |
| کانتینر‌سازی | Docker |
| FrontEnd| Blazor WebAssembly |

---

## ✨ امکانات (Features)

### 👤 حساب کاربری
- ثبت‌نام و ورود کاربر
- احراز هویت مبتنی بر JWT
- ویرایش اطلاعات حساب کاربری
- تغییر رمز عبور
- مشاهده اطلاعات پنل کاربری

### 🛍️ محصولات و دسته‌بندی‌ها
- مشاهده لیست محصولات و دسته‌بندی‌ها
- افزودن محصول به لیست علاقه‌مندی‌ها (Favorites)
- مشاهده لیست علاقه‌مندی‌های کاربر

### 🛒 سفارش‌ها
- افزودن محصول به سبد خرید / ثبت سفارش
- مشاهده فاکتور سفارش در پنل کاربری
- مشاهده تاریخچه سفارش‌ها
- اعمال کد تخفیف روی سفارش

### 🛠️ پنل مدیریت (Admin)
- مدیریت کاربران (لیست، افزودن، ویرایش، حذف) با صفحه‌بندی و فیلتر
- مدیریت نقش‌ها (Roles) و دسترسی‌ها (Permissions)
- مدیریت محصولات و دسته‌بندی‌ها (شامل آپلود تصویر محصول)
- مدیریت کدهای تخفیف (افزودن، ویرایش، مشاهده) با پشتیبانی از تاریخ شمسی

---

## ⚙️ راه‌اندازی پروژه (Getting Started)

### پیش‌نیازها
- [.NET SDK](https://dotnet.microsoft.com/download) (نسخه مطابق با `Luman.Api.csproj`)
- SQL Server (یا SQL Server LocalDB)
- Visual Studio 2022 یا هر IDE دیگر مناسب دات‌نت

### مراحل نصب

1. کلون کردن ریپازیتوری:
```bash
git clone https://github.com/AhmadrezaGolmakani/LumanGameShope.git
cd LumanGameShope
```

2. تنظیم Connection String:

در فایل `Luman.Api/appsettings.json` (یا ترجیحاً از طریق [User Secrets](https://learn.microsoft.com/aspnet/core/security/app-secrets)) مقدار زیر را متناسب با محیط خودتان تنظیم کنید:
```json
"ConnectionStrings": {
  "luman": "Data Source=.;Initial Catalog=luman-DB;TrustServerCertificate=True;Integrated Security=true"
}
```

3. اعمال Migrationها و ساخت دیتابیس:
```bash
cd Luman.DataLayer
dotnet ef database update --startup-project ../Luman.Api
```

4. تنظیم مقادیر JWT در `appsettings.json`:
```json
"JwtSetting": {
  "Secret": "<یک کلید امن و منحصربه‌فرد جایگزین کنید>",
  "Issure": "luman",
  "Audience": "web Api"
}
```
> ⚠️ مقدار `Secret` را هرگز در ریپازیتوری عمومی commit نکنید؛ از User Secrets یا Environment Variables استفاده کنید.

5. اجرای پروژه:
```bash
cd ../Luman.Api
dotnet run
```

6. مستندات Swagger به‌صورت پیش‌فرض روی مسیر ریشه در دسترس است:
```
https://localhost:{PORT}/
```

---

## 🐳 اجرا با Docker

پروژه شامل `Dockerfile.txt` برای Containerize کردن است. برای build و اجرا:

```bash
docker build -t luman-gameshop -f Dockerfile.txt .
docker run -p 8080:80 luman-gameshop
```

---

## 📚 مستندات API

پس از اجرای پروژه، مستندات کامل و تعاملی API (شامل تمام اندپوینت‌ها، مدل‌های ورودی/خروجی و امکان تست مستقیم) از طریق Swagger UI در دسترس است.

نمونه‌ای از گروه‌بندی نسخه‌های API:

| نسخه | حوزه |
|---|---|
| v1.0 | حساب کاربری، محصولات (نمایش عمومی)، سفارش‌ها |
| v2.0 | پنل مدیریت (کاربران، محصولات، تخفیف‌ها) |

---

## 🗂️ ساختار دیتابیس (خلاصه)

موجودیت‌های اصلی دیتابیس:

`User` · `Role` · `UserRole` · `Permition` · `RolePermission` · `Product` · `Category` · `CategoryProduct` · `FavoriteProduct` · `Order` · `OrderDetails` · `Discount`

---

## 🗺️ نقشه راه (Roadmap)

- [ ] پیاده‌سازی Refresh Token
- [ ] افزودن Rate Limiting روی اندپوینت‌های حساس (Login/Register)
- [ ] پیاده‌سازی تأیید ایمیل هنگام ثبت‌نام
- [ ] افزودن Logging ساخت‌یافته (Serilog)
- [ ] تبدیل عملیات دیتابیس به Async
- [ ] افزودن تست‌های واحد (Unit Tests)

---

## 👨‍💻 توسعه‌دهنده

**احمدرضا گلمکانی‌نیا**
برنامه‌نویس فول‌استک دات‌نت

- GitHub: [@AhmadrezaGolmakani](https://github.com/AhmadrezaGolmakani)
- Email: golmakani.info@gmail.com

---

## 📄 لایسنس

این پروژه صرفاً جهت یادگیری و نمایش مهارت‌های برنامه‌نویسی توسعه داده شده است.
