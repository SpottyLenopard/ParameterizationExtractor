﻿import { Injectable } from '@angular/core'
import { Http, Response, RequestOptions, RequestOptionsArgs, Headers, ResponseContentType } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import { pDependentTable } from '../model/pDependentTable';
import { pTableMetadata } from '../model/pTableMetadata';
import { scriptsPackage } from '../model/scriptsPackage';
import { AlertService } from './alert.service'

@Injectable()
export class apiService {
    api: string;

    constructor(protected http: Http, private alertService: AlertService) {
        this.api = '/';
    }

    getRequestOptionArgs(connectionString?: string): RequestOptionsArgs {
        let options = new RequestOptions();
        options.headers = new Headers();

        options.headers.append('Content-Type', 'application/json');

        if (connectionString != null)
            options.headers.append('ConnectionString', connectionString);
       
        return options;
    }


    private intercept(observable: Observable<Response>): Observable<Response> {
        return observable.catch((err, s) => {          
                this.alertService.error(err);
                return Observable.throw(err);
            });
    }


    getDependentTables(tableName: string, connectionString: string): Observable<pDependentTable[]> {
        return (
            this.intercept(this
                .http
                .get(this.api + 'Schema/DependentTables/' + tableName, this.getRequestOptionArgs(connectionString))
            )
                .map((r: Response) => r.json() || {}));     
        
            
    }

    getTableMetaData(tableName: string, connectionString: string): Observable<pTableMetadata> {
        return (
            this.intercept(this
                .http
                .get(this.api + 'Schema/TableMetaData/' + tableName, this.getRequestOptionArgs(connectionString))
            )
                .map((r: Response) => r.json() || {})
        );
    }  

    executePackage(connectionString: string, pack: scriptsPackage): Observable<Blob> {
        let opt = this.getRequestOptionArgs(connectionString);
        opt.responseType = ResponseContentType.ArrayBuffer;
        opt.headers.append('Accept', 'application/zip');

        //new Blob([d], { type: "application/zip" })
        return (
            this.intercept(
                 this.http
                    .post(this.api + 'PackageProcessor/Execute', pack, opt)
                )
                .map(response => {
                    return new Blob([response.blob()], { type: "application/zip" });
                })
        );
    }   
}