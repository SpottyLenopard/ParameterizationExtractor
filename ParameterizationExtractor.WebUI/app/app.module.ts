import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { routes } from './app.routes';
import { RouterModule } from '@angular/router';
import { HomeModule } from './home/home.module';
import { SharedModule } from './shared/shared.module';
import { FeaturesModule } from './features/features.module';
import { ExtractorModule } from './extractor/extractor.module';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'

import { HeaderComponent } from './shared/screen/header.component';
import { FooterComponent } from './shared/screen/footer.component';
import { AppComponent } from './app.component';

@NgModule({
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        SharedModule,
        HomeModule,
        BrowserModule,
        ExtractorModule,
        BrowserAnimationsModule,
        FeaturesModule,
        RouterModule.forRoot(routes, { useHash: true })        
    ],
    declarations: [AppComponent],
    exports: [FormsModule],
    bootstrap: [AppComponent]
})
export class AppModule { }