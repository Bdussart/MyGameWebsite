import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        let errorMessage = 'An error happened';
        
        // Vérifie si l'erreur a un corps
        if (error.error) {
            if (error.error instanceof Error) {
              // A client-side or network error occurred. Handle it accordingly.
              errorMessage = `An error occurred: ${error.error.message}`; 
            } else {
              // The backend returned an unsuccessful response code.
              // The response body may contain clues as to what went wrong,
              errorMessage = (`Backend returned code ${error.status}, body was: ${JSON.stringify(error.error)}`);
            }
        }

        // Retourne une erreur observable
        return throwError(() => new Error(errorMessage));
      })
    );
  }
}