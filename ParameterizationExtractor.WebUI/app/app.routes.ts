import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component'
import { ExtractorComponent } from './extractor/extractor.component'
import { DownloadPageComponent } from './features/downloadpage.component'


export const routes: Routes = [
    {
        path: '',
        redirectTo: '/home',
        pathMatch: 'full'
    }
    ,{
        path: 'home',
        component: HomeComponent
    },
    {
        path: 'extractor',
        component: ExtractorComponent
    },
    {
        path: 'download',
        component: DownloadPageComponent
    }

];