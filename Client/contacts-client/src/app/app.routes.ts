import { Routes } from '@angular/router';
import { Login } from './pages/login/login';
import { ContactsList } from './pages/contacts-list/contacts-list';
import { ContactDetail } from './pages/contact-detail/contact-detail';
import { ContactForm } from './pages/contact-form/contact-form';
import { AuthGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: 'login', component: Login },
  { path: 'contacts', component: ContactsList },
  { path: 'contacts/add', component: ContactForm, canActivate: [AuthGuard] },
  { path: 'contacts/edit/:id', component: ContactForm, canActivate: [AuthGuard] },
  { path: 'contacts/:id', component: ContactDetail },
  { path: '', redirectTo: '/contacts', pathMatch: 'full' },
  { path: '**', redirectTo: '/contacts' }
];

