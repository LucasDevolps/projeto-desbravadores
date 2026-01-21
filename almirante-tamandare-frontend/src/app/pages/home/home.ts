import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SectionsComponent } from '../../shared/components/sections/sections';
@Component({
  selector: 'app-home',
  imports: [CommonModule, SectionsComponent],
  templateUrl: './home.html',
  styleUrl: './home.scss',
})
export class Home {

}
