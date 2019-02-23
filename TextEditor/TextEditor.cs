using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wintellect.PowerCollections;

class TextEditor : ITextEditor
{
    private List<string> users;
    private Trie<BigList<char>> usersAndText;
    private Trie<Stack<string>> cache;

    public TextEditor()
    {
        this.users = new List<string>();
        this.usersAndText = new Trie<BigList<char>>();
        this.cache = new Trie<Stack<string>>();
    }

    public void Clear(string username)
    {
        this.LoggedInCheck(username);
        this.Cache(username);
        this.usersAndText.GetValue(username).Clear();
    }

    public void Delete(string username, int startIndex, int length)
    {
        this.LoggedInCheck(username);
        this.Cache(username);
        this.usersAndText.GetValue(username).RemoveRange(startIndex, length);
    }

    public void Insert(string username, int index, string str)
    {
        this.LoggedInCheck(username);
        this.Cache(username);
        this.usersAndText.GetValue(username).InsertRange(index,str);
    }

    public int Length(string username)
    {
        this.LoggedInCheck(username);
        return this.usersAndText.GetValue(username).Count;
    }

    public void Login(string username)
    {
        this.users.Add(username);
        this.usersAndText.Insert(username, new BigList<char>());
        this.cache.Insert(username, new Stack<string>());
    }

    public void Logout(string username)
    {
        this.users.Remove(username);
        this.usersAndText.Insert(username, null);
    }

    public void Prepend(string username, string str)
    {
        this.LoggedInCheck(username);
        this.Cache(username);
        this.usersAndText.GetValue(username).AddRangeToFront(str);
    }

    public string Print(string username)
    {
        this.LoggedInCheck(username);
        return string.Join("",this.usersAndText.GetValue(username));
    }

    public void Substring(string username, int startIndex, int length)
    {
        this.LoggedInCheck(username);
        this.Cache(username);
        BigList<char> substring = this.usersAndText.GetValue(username).GetRange(startIndex, length);
        this.usersAndText.Insert(username, substring);
    }

    public void Undo(string username)
    {
        this.LoggedInCheck(username);
        this.usersAndText.Insert(username,new BigList<char>(this.cache.GetValue(username).Pop()));
    }

    public IEnumerable<string> Users(string prefix = "")
    {
        List<string> users = this.users.Where(x => x.StartsWith(prefix)).ToList();
        return users;
    }

    private void Cache(string username)
    {
        this.cache.GetValue(username).Push(string.Join("", this.usersAndText.GetValue(username)));
    }

    private void LoggedInCheck(string username)
    {
        if (!this.users.Contains(username))
        {
            throw new UnauthorizedAccessException();
        }
    }
}
