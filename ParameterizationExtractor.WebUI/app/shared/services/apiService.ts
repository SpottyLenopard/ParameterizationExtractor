import { Injectable } from '@angular/core'
import { Http, Response, RequestOptions, RequestOptionsArgs, Headers, ResponseContentType } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import 'rxjs/add/operator/map';
import { pDependentTable } from '../model/pDependentTable';
import { pTableMetadata } from '../model/pTableMetadata';
import { scriptsPackage } from '../model/scriptsPackage';

@Injectable()
export class apiService {
    api: string;

    constructor(protected http: Http) {
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


    getDependentTables(tableName: string, connectionString: string): Observable<pDependentTable[]> {
        return this
            .http
            .get(this.api + 'Schema/DependentTables/' + tableName, this.getRequestOptionArgs(connectionString))
            .map((r: Response) => r.json() || {});
    }

    getTableMetaData(tableName: string, connectionString: string): Observable<pTableMetadata> {
        return this
            .http
            .get(this.api + 'Schema/TableMetaData/' + tableName, this.getRequestOptionArgs(connectionString))
            .map((r: Response) => r.json() || {});
    }  

    executePackage(connectionString: string, pack: scriptsPackage): Observable<Blob> {
        let opt = this.getRequestOptionArgs(connectionString);
        opt.responseType = ResponseContentType.ArrayBuffer;
        opt.headers.append('Accept', 'application/zip');

        //new Blob([d], { type: "application/zip" })
        return this.http
            .post(this.api + 'PackageProcessor/Execute', pack, opt)
            .map(response => {
                //let file = response.blob();

                return new Blob([response.text()], { type: "application/zip" });
            });
    }   
}