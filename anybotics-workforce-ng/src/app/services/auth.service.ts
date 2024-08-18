import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { signInWithEmailAndPassword, signOut, onAuthStateChanged } from 'firebase/auth';
import { getFirestore, doc, getDoc } from 'firebase/firestore';
import { auth } from '../firebase-config';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private loggedIn$ = new BehaviorSubject<boolean>(false);
  private userRole$ = new BehaviorSubject<string | null>(null);
  private initialized$ = new BehaviorSubject<boolean>(false);
  private db = getFirestore();
  private firebaseToken?: string;

  get isLoggedIn$(): Observable<boolean> {
    return this.loggedIn$.asObservable();
  }

  get currentUserRole$(): Observable<string | null> {
    return this.userRole$.asObservable();
  }

  get isInitialized$(): Observable<boolean> {
    return this.initialized$.asObservable();
  }

  get accessToken() {
    return this.firebaseToken;
  }

  login(email: string, password: string): Promise<void> {
    return signInWithEmailAndPassword(auth, email, password)
      .then(() => this.updateUserState(email))
      .catch((error) => {
        console.error('Login error', error);
        this.clearUserState();
      });
  }

  logout(): Promise<void> {
    return signOut(auth)
      .then(() => this.clearUserState())
      .catch((error) => console.error('Logout error', error));
  }

  initializeAuthState(): Promise<void> {
    return new Promise((resolve) => {
      onAuthStateChanged(auth, async (user) => {
        if (user) {
          this.firebaseToken = await user.getIdToken();
          await this.updateUserState(user.email!);
        } else {
          this.clearUserState();
        }
        this.initialized$.next(true);
        resolve(); // Resolving the promise after initialization
      });
    });
  }

  private async updateUserState(email: string): Promise<void> {
    const role = await this.fetchUserRole(email);
    this.loggedIn$.next(true);
    this.userRole$.next(role);
  }

  private clearUserState(): void {
    this.firebaseToken = undefined;
    this.loggedIn$.next(false);
    this.userRole$.next(null);
  }

  private async fetchUserRole(email: string): Promise<string | null> {
    try {
      const docRef = doc(this.db, 'userRoles', email);
      const docSnap = await getDoc(docRef);

      if (docSnap.exists()) {
        return docSnap.data()['role'] || null;
      } else {
        console.log('No such document!');
        return null;
      }
    } catch (error) {
      console.error('Error fetching user role:', error);
      return null;
    }
  }
}
