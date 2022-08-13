using System.Text;
using System.Text.RegularExpressions;

public enum PasswordScore
{
    Blank = 0,
    VeryWeak = 1,
    Medium = 2,
    Strong = 3,
    VeryStrong = 4
}

public class PasswordAdvisor
{
    public static bool ContainCharacter(string contain, string charlist)
    {
        for(int i =0; i< charlist.Length; ++i)
        {
            if (contain.Contains(charlist.Substring(i, 1)))
                return true;
        }
        return false;
    }
    public static PasswordScore CheckStrength(string password)
    {
        int score = 0;

        if (password.Length < 1)
            return PasswordScore.Blank;
        if (password.Length < 6)
            return PasswordScore.VeryWeak;
        if (ContainCharacter(password,"0987654321") )  //number only //"^\d+$" if you need to match more than one digit.
            score++;
        if (ContainCharacter(password, "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM")) //both, lower and upper case
            score++;
        if (ContainCharacter(password, "!,@,#,$,%,^,&,*,?,_,~,-,£,(,),+,-"))  
            score++;
        return (PasswordScore)score;
    }
    public static  bool PasswordIsStrong(string password)
    {
        if (CheckStrength( password)>= PasswordScore.Medium)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}