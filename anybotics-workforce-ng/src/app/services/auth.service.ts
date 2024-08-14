import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { signInWithEmailAndPassword, signOut, onAuthStateChanged } from 'firebase/auth';
import { getFirestore, doc, getDoc } from 'firebase/firestore';
import { auth } from '../firebase-config';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private loggedIn = new BehaviorSubject<boolean>(false);
  private userRole = new BehaviorSubject<string | null>(null);
  
  private db = getFirestore();

  constructor() {
    onAuthStateChanged(auth, async (user) => {
      if (user) {
        this.loggedIn.next(true);
        const role = await this.fetchUserRole(user.email!); // Fetch and store the user role
        this.userRole.next(role);
      } else {
        this.loggedIn.next(false);
        this.userRole.next(null); // Clear the role when the user logs out
      }
    });
  }

  get isLoggedIn() {
    return this.loggedIn.asObservable();
  }

  get currentUserRole() {
    return this.userRole.asObservable();
  }

  login(email: string, password: string) {
    return signInWithEmailAndPassword(auth, email, password)
      .then(async () => {
        this.loggedIn.next(true);
        const role = await this.fetchUserRole(email); // Fetch and store the user role
        this.userRole.next(role);
      })
      .catch((error) => {
        console.error('Login error', error);
        this.loggedIn.next(false);
        this.userRole.next(null);
      });
  }

  logout() {
    return signOut(auth)
      .then(() => {
        this.loggedIn.next(false);
        this.userRole.next(null); // Clear the role on logout
      })
      .catch((error) => console.error('Logout error', error));
  }

  // Fetch the user's role from Firestore
  private async fetchUserRole(email: string): Promise<string | null> {
    const docRef = doc(this.db, 'userRoles', email);
    const docSnap = await getDoc(docRef);

    if (docSnap.exists()) {
      return docSnap.data()['role'] || null;
    } else {
      console.log('No such document!');
      return null;
    }
  }
}
