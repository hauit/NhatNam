# AdminLTE

AdminLTE - is a Free Premium Admin control Panel Theme Based On Bootstrap 3.x

the creator of AdminLTE is [Abdulllah Almsaeed](https://adminlte.io/about)

follow this [AdminLTE](https://github.com/almasaeed2010/AdminLTE) link for original HTML/JavaScript version.


# ASP.NET MVC Version

this repo, provide full **ASP.NET MVC** version of AdminLTE, consisting all the demo pages provided by HTML/JavaScript version of AdminLTE such as:

- Dashboard
- Layout
- Widgets
- Charts
- UI Elements
- Forms
- Tables
- Calendar
- Mailbox
- Examples
- Multilevel

![adminltemvc](adminlte/Content/adminltemvc.png)

# Development Tools & Environment

I'm using **Visual Studio Community 2015** for the development tools on Windows 10 machine. For this project, i'm using AdminLTE version **2.4.0**.

# AdminLTE ASP.NET MVC Version Usage

You can **Clone / Download** the repo and then start building beautiful web app using this visual studio solution. From this project you can also learn how to:

- work with controller
- work with view
- work with partial view
- work with layout
- work with JavaScript / jQuery
- work with razor
- create HTML helper extension

# Supported by CodeRush.Co
[CodeRush.CO] source code collections (https://coderush.co). 50% Off All Products, Use Discount Code **GITHUB50**



# Create friend url
## IIS fix
- Đôi iis mặc định sẽ chặn các url có đuôi .html. Để khắc phục thì cần thêm thẻ sau vào web config 
```

system.webServer>
  <rewrite>
    <rules>
      <rule name="RewriteHtmlToMvc" stopProcessing="true">
        <match url="(.*)\.html$" ignoreCase="true" />
        <action type="Rewrite" url="{R:1}" />
      </rule>
    </rules>
  </rewrite>
</system.webServer>

```

# Sửa web.config 
- <modules runAllManagedModulesForAllRequests="true" />

## Route config cho url
- Gọi ***routes.MapMvcAttributeRoutes();*** trong route config 

## Thêm attribute để tạo link
- Thêm attribute vào controller name: [RoutePrefix("Trang-chu")]
- Thêm attribute vào action name: [Route("Index.html")]
- Với các url động theo tên cảu tham số thì thêm attribute vào action như sau: [Route("Thong-bao/{caption}-{content?}.html")] với caption, content là tham số đầu vào của action

## Để kiểm tra các controller và action có được load đầy đủ hay không thì có thể kiểm tra bằng RouteDebuggig
- cài đặt: Install-Package RouteDebugger
- gỡ cài đặt: Uninstall-Package RouteDebugger -RemoveDependencies