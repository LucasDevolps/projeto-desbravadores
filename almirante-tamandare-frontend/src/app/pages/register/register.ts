import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InputComponent } from '../../shared/components/input/input';
import { ButtonComponent } from '../../shared/components/button/button';
import { SelectComponent } from '../../shared/components/select/select';


@Component({
  selector: 'app-register',
  imports: [CommonModule, InputComponent, ButtonComponent, SelectComponent],
  templateUrl: './register.html',
  styleUrl: './register.scss',
})
export class Register {

}
