import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable, catchError, retry, throwError } from 'rxjs';

export default abstract class ApiRepository{
    protected completedEndpoint: string;

    protected get headers() {
        return {
            headers: new HttpHeaders({
                accept: 'application/json'
            }),
        };
    }

    constructor(
        protected readonly _apiOrigin: string,
        protected readonly _endpoint: string,
        protected readonly _http: HttpClient
    ){
        this.completedEndpoint = `${this._apiOrigin}/${this._endpoint}`;
    }

    protected get<T>(extraResource = '', query = ''): Observable<T> {
        query = query ? `?${query}` : '';
        extraResource = extraResource ? `/${extraResource}` : '';

        return this._http
            .get<T>(`${this.completedEndpoint}${extraResource}${query}`, this.headers)
            .pipe(retry(3), catchError(this.handleError));
    }

    protected post<T>(body: any, extraResource = '', headers?: any): Observable<T> {
        extraResource = extraResource ? `/${extraResource}` : '';
        const url = `${this.completedEndpoint}${extraResource}`;

        if (!headers) {
            if (!(body instanceof FormData)) {
                headers = new HttpHeaders({
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                });
            } else {
                headers = undefined;
            }
        }


        return this._http
            .post<T>(url, body, { headers })
            .pipe(retry(3), catchError(this.handleError));
    }

    protected patch<T>(extraResource = '', body: any = null): Observable<T> {
        extraResource = extraResource ? `/${extraResource}` : '';

        return this._http
            .patch<T>(`${this.completedEndpoint}${extraResource}`, body, {
                headers: new HttpHeaders({
                    'Content-Type': 'application/json',
                    'Accept': 'application/json',
                })
            })
            .pipe(retry(3), catchError(this.handleError));
    }

    protected putById<T>(
        id: string,
        body: any = null,
        extraResource = ''
    ): Observable<T> {
        extraResource = extraResource ? `/${extraResource}` : '';

        return this._http
            .put<T>(`${this.completedEndpoint}/${id}${extraResource}`, body, this.headers)
            .pipe(retry(3), catchError(this.handleError));
    }

    protected delete<T>(extraResource = '', query = ''): Observable<T> {
        query = query ? `?${query}` : '';

        extraResource = extraResource ? `/${extraResource}` : '';

        return this._http
            .delete<T>(`${this.completedEndpoint}${extraResource}${query}`, this.headers)
            .pipe(retry(3), catchError(this.handleError));
    }

    protected handleError(error: HttpErrorResponse) {
        if (error.status === 200) {
            return throwError(() => ({
                status: error.status,
                message: error.error?.message || 'Operation successful, but the backend returned an unexpected object.'
            }));
        }

        let errorMessage = 'Something wrong happened, try later.';
        if (error.error instanceof ErrorEvent) {
            console.error('An error ocurred: ', error.error.message);
            errorMessage = error.error.message;
        } else {
            console.error(
                `Backend error: ${error.status}, ` + ` with body: ${error.error}`
            );
            errorMessage = error.error.message || errorMessage;
        }
        return throwError({ status: error.status, message: errorMessage });
    }
}