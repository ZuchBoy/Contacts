//import { Component, signal } from '@angular/core';
//import { RouterOutlet } from '@angular/router';


// Source - https://stackoverflow.com/a
// Posted by Yogesh Waghmare
// Retrieved 2025-11-23, License - CC BY-SA 4.0

import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterModule],
  templateUrl: './app.html',
  styleUrls: ['./app.scss']
})
export class App{
}


/*
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('contacts-client');*/

