import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

export interface Contact {
  id: number;
  firstName: string;
  surname: string;
  email: string;
  category: string;
  subcategory?: string;
  phone?: string;
  birthDate?: string;
}

@Injectable({ providedIn: 'root' })
export class ContactService {
  private base = `${environment.apiUrl}/Contact`;
  constructor(private http: HttpClient) { }

  getAll(): Observable<Contact[]> {
    return this.http.get<Contact[]>(this.base);
  }

  get(id: number) {
    return this.http.get<Contact>(`${this.base}/${id}`);
  }

  create(payload: any) {
    return this.http.post<Contact>(this.base, payload);
  }

  // ToDo: update this function
  update(id: number, payload: any) {
    return this.http.put<void>(`${this.base}/${id}`, payload);
  }

  delete(id: number) {
    return this.http.delete<void>(`${this.base}/${id}`);
  }

}
