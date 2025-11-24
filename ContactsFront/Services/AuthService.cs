using ContactsFront.Api;
using ContactsFront.Models;
using Microsoft.JSInterop;

namespace ContactsFront.Services;

public class AuthService
{
    private readonly IJSRuntime _js;
    private string? _cachedToken;
    private const string TokenKey = "auth_token";
    private readonly HttpClient _http;
    private readonly CustomAuthStateProvider _authStateProvider;

    public AuthService(HttpClient http, IJSRuntime js, CustomAuthStateProvider authStateProvider)
    {
        _http = http;
        _js = js;
        _authStateProvider = authStateProvider;
    }

    public async Task<bool> LoginAsync(LoginModel login)
    {
        var resp = await _http.PostAsJsonAsync("/auth/login", login);
        if (!resp.IsSuccessStatusCode) return false;

        var result = await resp.Content.ReadFromJsonAsync<AuthResult>();
        if (result == null || string.IsNullOrEmpty(result.Token)) return false;

        await _js.InvokeVoidAsync("localStorage.setItem", TokenKey, result.Token);
        _cachedToken = result.Token;

        _authStateProvider.NotifyUserAuthentication(result.Token);
        return true;
    }

    public async Task LogoutAsync()
    {
        _cachedToken = null;
        await _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        _authStateProvider.NotifyUserLogout();
    }

    public async Task<string?> GetTokenSafeAsync()
    {
        if (_cachedToken != null)
            return _cachedToken;

        try
        {
            _cachedToken = await _js.InvokeAsync<string?>("localStorage.getItem", TokenKey);
        }
        catch
        {
            return null;
        }

        return _cachedToken;
    }

    public async Task<bool> IsLoggedInAsync()
    {
        var t = await GetTokenSafeAsync();
        return !string.IsNullOrEmpty(t);
    }

    public async Task<HttpResponseMessage> RegisterAsync(RegisterModel register)
    {
        if (string.IsNullOrEmpty(register.Password) || !IsValidPassword(register.Password))
        {
            throw new MissingFieldException("Registration failed. Password must be at least 8 characters, contain a number, an upper character and a special character.");
        }

        return await _http.PostAsJsonAsync("/Auth/register", register);
    }

    private bool IsValidPassword(string password)
    {
        if (password.Length < 8)
            return false;

        if (!password.Any(char.IsDigit))
            return false;

        if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
            return false;

        if (!password.Any(char.IsUpper))
            return false;

        return true;
    }
}
