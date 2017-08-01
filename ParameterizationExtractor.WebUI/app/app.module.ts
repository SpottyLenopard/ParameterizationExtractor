import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { routes } from './app.routes';
import { RouterModule } from '@angular/router';
import { HomeModule } from './home/home.module';
import { SharedModule } from './shared/shared.module';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

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
        RouterModule.forRoot(routes, { useHash: true })        
    ],
    declarations: [AppComponent],
    exports: [FormsModule],
    bootstrap: [AppComponent]
})
export class AppModule { }