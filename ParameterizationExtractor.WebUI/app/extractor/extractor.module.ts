import { NgModule } from '@angular/core';
import { ExtractorComponent } from './extractor.component';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../shared/shared.module';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BusyModule } from 'angular2-busy';

@NgModule({
    declarations: [ExtractorComponent],
    exports: [ExtractorComponent],
    imports: [SharedModule, CommonModule, FormsModule, BusyModule]
})
export class ExtractorModule {

}