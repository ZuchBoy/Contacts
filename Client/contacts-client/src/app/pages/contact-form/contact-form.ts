import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ContactService } from "../../services/contact.service";
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'

@Component({
  selector: 'app-contact-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './contact-form.html',
  styleUrl: './contact-form.scss',
})
export class ContactForm implements OnInit {
  model: any = {
    firstname: '',
    surname: '',
    email: '',
    category: '',
    subcategory: '',
    phone: '',
    birthDate: ''
  };

  isEdit = false;
  id ?: number;
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private service: ContactService
  ) { }

  ngOnInit() {
    const param = this.route.snapshot.paramMap.get('id');

    if (param) {
      this.id = Number(param);
      this.isEdit = true;
      this.service.get(this.id).subscribe(res => this.model = res);
    }
  }

  save() {
    if (this.isEdit && this.id) {
      this.service.update(this.id, this.model).subscribe(() => this.router.navigate(['/contacts']));
    } else {
      this.service.create(this.model).subscribe(() => this.router.navigate(['/contacts']));
    }
  }
}
