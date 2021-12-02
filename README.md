<h1 align="center">Basic Shop (WPF)</h1>

<p align="center">
    <a href="/./LICENSE"><img src="https://img.shields.io/github/license/VegetaTheKing/BasicShop"></a>
</p>

<h5 align="center">
    DISCLAIMER: This project was made as a school project and so, it's UI is in the Polish language. Sorry folks :/
</h5>

<details open="open">
  <summary>Table of Contents</summary>
  <ol>
    <li><a href="#about">About</a></li>
    <li>
        <a href="#getting-started">Getting Started</a>
        <ul>
            <li><a href="#project-structure">Project structure</a></li>
            <li><a href="#set-up">Set up</a></li>
        </ul>
    </li>
    <li><a href="#documentation">Documentation</a></li>
    <li><a href="#license">License</a></li>
  </ol>
</details>

## About

This project is basically a small shop. The whole logic is on the client-side, in a real-life application, I would've used some API to manage privileges.
It was my first big project using .NET Framework. The app supports viewing products, searching, filtering lists, ordering, wishlist, checkout and admin panel to manage everything.

## Getting Started

### Project structure

`BasicShop` - .NET Framework 4.7.2 WPF project<br>
`BasicShop.Tests` - .NET Framework 4.7.2 MSTests project<br>
`BasicShop.FilesServer` - ASP.NET Core 3.1 files server project<br>
`DatabaseBackups` - Folder with database backups as SQL and .bak<br>
`Docs` - Folder with documentation

### Set up

##### Application projects
1. Clone repository `git clone https://github.com/VegetaTheKing/BasicShop.git`
2. Open `BasicShop.sln`

##### MS SQL database
1. Clone repository `git clone https://github.com/VegetaTheKing/BasicShop.git`
2. Open Microsoft SQL Server Management Studio
3. Login to server
4. Add new login to SQL Server named `shop_admin` with password `Admin12345`
5. Databases->RMB->Restore database, select device and then navigate to `DatabaseBackups/shop_database.bak`

`NOTE: Connection string is in`<a href="https://github.com/VegetaTheKing/BasicShop/blob/master/BasicShop/DatabaseCustomConnection.cs">`DatabaseCustomConnection.cs`</a>`and files server addres in`<a href="https://github.com/VegetaTheKing/BasicShop/blob/master/BasicShop/App.config#L11">`App.config`</a>

## Documentation

<table>
    <tr>
        <th style="text-align:center"><a href="/./docs/img/erd.png"><img alt="erd" src="/./docs/img/erd.png"></a></th>
    </tr>
    <tr>
        <td align="center">Database diagram</td>
    </tr>
</table>


## [License](/./LICENSE)

```
MIT License

Copyright (c) 2021 Mateusz Ciaglo

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```