import { NgModule } from '@angular/core';
import { DownloadPageComponent } from './downloadpage.component';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../shared/shared.module';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BusyModule } from 'angular2-busy';

@NgModule({
    declarations: [DownloadPageComponent],
    exports: [DownloadPageComponent],
    imports: [SharedModule, CommonModule, FormsModule, BusyModule]
})
export class FeaturesModule {

}