// Decompiled with JetBrains decompiler
// Type: GoogleBot.Program
// Assembly: GoogleBot, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5CC22185-F24F-4FA5-9B39-7CC9023B83A2
// Assembly location: C:\Users\leand\Desktop\GoogleBOT\GoogleBot.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using WatiN.Core;

namespace GoogleBot
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      string pathKeyWords = Program.Initialize(string.Empty);
      Console.WriteLine("Iniciando pesquisas");
      Program.SearchAndGoToRandomLink(pathKeyWords);
    }

    private static void SearchAndGoToRandomLink(string pathKeyWords)
    {
      using (StreamReader streamReader = new StreamReader(pathKeyWords))
      {
        string empty = string.Empty;
        string outputFolder = Program.CreateOutputFolder();
        string str;
        while (!string.IsNullOrEmpty(str = streamReader.ReadLine()))
        {
          FireFox browser = (FireFox) null;
          try
          {
            Console.WriteLine(string.Format("Consultando palavra chave:{0}", (object) str));
            string fileName = str.Trim().Replace(" ", "+");
            browser = new FireFox(string.Format("https://www.google.com.br/search?q={0}", (object) fileName));
            Program.CreateOutputFile(fileName, browser, outputFolder);
            Console.WriteLine();
            List<string> searchLinks = Program.FindSearchLinks(browser);
            Console.WriteLine();
            string url = Program.ChooseRandomLink(searchLinks);
            browser.GoTo(url);
            Thread.Sleep(1000);
            browser.Close();
            Thread.Sleep(1000);
          }
          catch
          {
            Console.WriteLine("There was an unhadled error, but I'll continue working Seymour.");
          }
          finally
          {
            try
            {
              browser?.Close();
            }
            catch
            {
            }
          }
        }
      }
    }

    private static string ChooseRandomLink(List<string> links)
    {
      if (links.Count > 0)
      {
        Console.WriteLine(string.Format("Choosing randomly one of the {0} links.", (object) links.Count));
        int index = new Random().Next(0, links.Count);
        return links[index];
      }
      Console.WriteLine("No links found for your search.");
      return string.Empty;
    }

    private static List<string> FindSearchLinks(FireFox browser)
    {
      ElementCollection elementCollection = browser.ElementsWithTag("h3", (string[]) null);
      // ISSUE: reference to a compiler-generated field
      if (Program.CS\u0024\u003C\u003E9__CachedAnonymousMethodDelegate1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: method pointer
        Program.CS\u0024\u003C\u003E9__CachedAnonymousMethodDelegate1 = new Func<Element, bool>((object) null, __methodptr(\u003CFindSearchLinks\u003Eb__0));
      }
      // ISSUE: reference to a compiler-generated field
      Func<Element, bool> anonymousMethodDelegate1 = Program.CS\u0024\u003C\u003E9__CachedAnonymousMethodDelegate1;
      IEnumerable<Element> elements = (IEnumerable<Element>) Enumerable.Where<Element>((IEnumerable<M0>) elementCollection, (Func<M0, bool>) anonymousMethodDelegate1);
      List<string> stringList = new List<string>();
      foreach (Element element in elements)
      {
        if (Regex.IsMatch(element.InnerHtml.ToString(), "href=\\\".*\\\""))
        {
          string input = Regex.Match(element.InnerHtml.ToString(), "href=\\\".*\\\"").Captures[0].ToString().Replace("href=\"", "").Replace("\"", "");
          if (Regex.IsMatch(input, "/url.*&amp;sa="))
          {
            string str = HttpUtility.UrlDecode(Regex.Match(input, "/url.*&amp;sa=").Captures[0].ToString().Replace("/url?q=", "").Replace("&amp;sa=", ""));
            Console.WriteLine(string.Format("Link found: {0}", (object) str));
            stringList.Add(str);
          }
        }
      }
      return stringList;
    }

    private static void CreateOutputFile(string fileName, FireFox browser, string outFolder)
    {
      string path = Path.Combine(outFolder, fileName + ".html");
      using (StreamWriter streamWriter = new StreamWriter(path))
      {
        if (!File.Exists(path))
          File.Create(path);
        streamWriter.Write(browser.Html.ToString());
      }
    }

    private static string CreateOutputFolder()
    {
      string path = Path.Combine("C:\\tmp\\", Convert.ToString(DateTime.Now.Day) + "_" + Convert.ToString(DateTime.Now.Month) + "_" + Convert.ToString(DateTime.Now.Year) + "___" + Convert.ToString(DateTime.Now.Hour) + "_" + (object) DateTime.Now.Minute + "_" + (object) DateTime.Now.Second);
      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);
      return path;
    }

    private static string Initialize(string pathKeyWords)
    {
      for (; string.IsNullOrEmpty(pathKeyWords) || !File.Exists(pathKeyWords); pathKeyWords = Console.ReadLine())
      {
        Console.Clear();
        Console.WriteLine("Digite o caminho da lista de palavras chave:");
      }
      return pathKeyWords;
    }
  }
}
