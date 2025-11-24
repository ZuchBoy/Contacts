import { Component, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { ContactService, Contact } from "../../services/contact.service";
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common'

@Component({
  selector: 'app-contacts-list',
  standalone: true,
  imports: [ CommonModule, RouterModule ],
  templateUrl: './contacts-list.html',
  styleUrls: ['./contacts-list.scss'],
})
export class ContactsList implements OnInit{
  contacts: Contact[] = [];
  constructor(
    private service: ContactService,
    private auth: AuthService,
    private router: Router
  ) { }

  ngOnInit() {
    this.load();
  }

  load() {
    this.service.getAll().subscribe({next: (res) => this.contacts = res})
  }

  delete(id: number) {
    if (confirm("Are you sure?")) {
      this.service.delete(id).subscribe(() => this.load());
    }
  }

  logout(){
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}
