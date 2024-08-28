import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { BehaviourSnippet } from "../models/behaviour-dto.model";
import { HttpClient } from "@angular/common/http";

@Injectable({
    providedIn: 'root'
})
export class BehaviourService {
    private apiUrl = 'http://localhost:3000/behaviours';

    constructor(private http: HttpClient) { }

    // Method to get all behaviour snippets
    getBehaviours(): Observable<BehaviourSnippet[]> {
        return this.http.get<BehaviourSnippet[]>(this.apiUrl);
    }
}