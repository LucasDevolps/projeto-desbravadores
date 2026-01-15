import { CommonModule } from '@angular/common';
import { Component, HostBinding, Input } from '@angular/core';

@Component({
  selector: 'app-input',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './input.html',
  styleUrl: './input.scss',
})
export class InputComponent {
  @Input() type: string = 'text';
  @Input() placeholder: string = '';
  @Input() label: string = '';
  @Input() iconSrc?: string;
  @Input() iconAlt: string = '';

  value: string = '';

  @HostBinding('class.has-value') get hasValue() {
    return this.value.length > 0;
  }

  onInput(e: Event) {
    this.value = (e.target as HTMLInputElement).value || '';
  }
}
