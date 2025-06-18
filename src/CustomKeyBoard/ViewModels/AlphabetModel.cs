using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CustomKeyBoard.ViewModels;

/// <summary>
///     自定义26个字母的虚拟键盘模型
/// </summary>
internal class AlphabetModel : INotifyPropertyChanged
{
    private string _a = "a";

    private string _b = "b";

    private string _c = "c";

    private string _d = "d";

    private string _e = "e";

    private string _f = "f";

    private string _g = "g";

    private string _h = "h";

    private string _i = "i";

    private string _j = "j";

    private string _k = "k";

    private string _l = "l";

    private string _m = "m";

    private string _n = "n";

    private string _o = "o";

    private string _p = "p";

    private string _q = "q";

    private string _r = "r";

    private string _s = "s";

    private string _t = "t";

    private string _u = "u";

    private string _v = "v";

    private string _w = "w";

    private string _x = "x";

    private string _y = "y";

    private string _z = "z";

    public string A
    {
        get => _a;
        set => SetField(ref _a, value);
    }

    public string B
    {
        get => _b;
        set => SetField(ref _b, value);
    }

    public string C
    {
        get => _c;
        set => SetField(ref _c, value);
    }

    public string D
    {
        get => _d;
        set => SetField(ref _d, value);
    }

    public string E
    {
        get => _e;
        set => SetField(ref _e, value);
    }

    public string F
    {
        get => _f;
        set => SetField(ref _f, value);
    }

    public string G
    {
        get => _g;
        set => SetField(ref _g, value);
    }

    public string H
    {
        get => _h;
        set => SetField(ref _h, value);
    }

    public string I
    {
        get => _i;
        set => SetField(ref _i, value);
    }

    public string J
    {
        get => _j;
        set => SetField(ref _j, value);
    }

    public string K
    {
        get => _k;
        set => SetField(ref _k, value);
    }

    public string L
    {
        get => _l;
        set => SetField(ref _l, value);
    }

    public string M
    {
        get => _m;
        set => SetField(ref _m, value);
    }

    public string N
    {
        get => _n;
        set => SetField(ref _n, value);
    }

    public string O
    {
        get => _o;
        set => SetField(ref _o, value);
    }

    public string P
    {
        get => _p;
        set => SetField(ref _p, value);
    }

    public string Q
    {
        get => _q;
        set => SetField(ref _q, value);
    }

    public string R
    {
        get => _r;
        set => SetField(ref _r, value);
    }

    public string S
    {
        get => _s;
        set => SetField(ref _s, value);
    }

    public string T
    {
        get => _t;
        set => SetField(ref _t, value);
    }

    public string U
    {
        get => _u;
        set => SetField(ref _u, value);
    }

    public string V
    {
        get => _v;
        set => SetField(ref _v, value);
    }

    public string W
    {
        get => _w;
        set => SetField(ref _w, value);
    }

    public string X
    {
        get => _x;
        set => SetField(ref _x, value);
    }

    public string Y
    {
        get => _y;
        set => SetField(ref _y, value);
    }

    public string Z
    {
        get => _z;
        set => SetField(ref _z, value);
    }

    // 通用的通知方法
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    // 切换大小写的方法
    public void SetUpperCase(bool isUpper)
    {
        A = isUpper ? "A" : "a";
        B = isUpper ? "B" : "b";
        C = isUpper ? "C" : "c";
        D = isUpper ? "D" : "d";
        E = isUpper ? "E" : "e";
        F = isUpper ? "F" : "f";
        G = isUpper ? "G" : "g";
        H = isUpper ? "H" : "h";
        I = isUpper ? "I" : "i";
        J = isUpper ? "J" : "j";
        K = isUpper ? "K" : "k";
        L = isUpper ? "L" : "l";
        M = isUpper ? "M" : "m";
        N = isUpper ? "N" : "n";
        O = isUpper ? "O" : "o";
        P = isUpper ? "P" : "p";
        Q = isUpper ? "Q" : "q";
        R = isUpper ? "R" : "r";
        S = isUpper ? "S" : "s";
        T = isUpper ? "T" : "t";
        U = isUpper ? "U" : "u";
        V = isUpper ? "V" : "v";
        W = isUpper ? "W" : "w";
        X = isUpper ? "X" : "x";
        Y = isUpper ? "Y" : "y";
        Z = isUpper ? "Z" : "z";
    }
}