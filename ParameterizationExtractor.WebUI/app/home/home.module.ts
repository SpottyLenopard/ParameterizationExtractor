import { NgModule } from '@angular/core';
import { HomeComponent } from './home.component';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../shared/shared.module';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@NgModule({
    declarations: [HomeComponent],
    exports: [HomeComponent],
    imports: [SharedModule, CommonModule, FormsModule]
})
export class HomeModule {

}