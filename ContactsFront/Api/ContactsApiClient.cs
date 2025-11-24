namespace ContactsFront.Api;

public class ContactsApiClient
{
    private readonly HttpClient _http;

    public ContactsApiClient(HttpClient http)
    {
        _http = http;
    }

    public void SetBearerToken(string token)
    {
        _http.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }

    // GET /Contact
    public async Task<List<Contact>?> GetContactsAsync()
    {
        return await _http.GetFromJsonAsync<List<Contact>>("/Contact");
    }

    // GET /Contact/{id}
    public async Task<Contact?> GetContactAsync(Guid id)
    {
        return await _http.GetFromJsonAsync<Contact>($"/Contact/{id}");
    }

    // PUT /Contact/{id}
    public async Task<HttpResponseMessage> UpdateContactAsync(Guid id, Contact contact)
    {
        return await _http.PutAsJsonAsync($"/Contact/{id}", contact);
    }

    // DELETE /Contact/{id}
    public async Task<HttpResponseMessage> DeleteContactAsync(Guid id)
    {
        return await _http.DeleteAsync($"/Contact/{id}");
    }
}

public class Category
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? RowVersion { get; set; }
    public List<Contact>? Contacts { get; set; }
    public List<Subcategory>? Subcategories { get; set; }
}

public class Subcategory
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public string? Name { get; set; }
    public string? RowVersion { get; set; }
    public Category? Category { get; set; }
    public List<Contact>? Contacts { get; set; }
}

public class User
{
    public Guid Id { get; set; }
    public Guid ContactId { get; set; }
    public string? Username { get; set; }
    public string? PwdHash { get; set; }
    public string? RowVersion { get; set; }
    public Contact? Contact { get; set; }
}

public class Contact
{
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public DateOnly? BirthDate { get; set; }
    public int CategoryId { get; set; }
    public int? SubcategoryId { get; set; }
    public string? RowVersion { get; set; }

    public Category? Category { get; set; }
    public Subcategory? Subcategory { get; set; }
    public List<User>? Users { get; set; }
}

public class RegisterModel {
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int CategoryId { get; set; }
}
